import flask                                # type: ignore
from flask import Flask, request            # type: ignore
from werkzeug.utils import secure_filename  # type: ignore
import os

import video_mediapipe                      # type: ignore

UPLOAD_FOLDER = './videos'
ALLOWED_EXTENSIONS = {'mp4', 'mov', 'txt'}

app = Flask(__name__)
app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER

# Check if the uploaded file's extension is an allowed one.
def is_allowed_file(filename):
    return '.' in filename and \
           filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS

@app.route('/upload', methods=['GET', 'POST'])
def upload_file():
    if request.method == 'POST':
        # check if the post request has the file part
        if 'file' not in request.files:
            return 'No file part'
        file = request.files['file']
        # If the user does not select a file, the browser submits an
        # empty file without a filename.
        if file.filename == '':
            return 'No selected file'
        if file and is_allowed_file(file.filename):
            filename = secure_filename(file.filename)

            video_name, video_extension = filename.split(".")
            save_name = video_name

            # To avoid file-name conflict.
            count = 1
            while os.path.exists(app.config["UPLOAD_FOLDER"] +f"/{save_name}.{video_extension}"):
                save_name = video_name + str(count)
                count += 1

            file_path = app.config["UPLOAD_FOLDER"] + f"/{save_name}.{video_extension}"
            file.save(file_path)

            sequence = video_mediapipe.detect_joints_from_video(file_path)

            with open(f"./mediapipe/{save_name}.txt", "a") as f:
                f.write(str(sequence))

            return get_files_json()
    return 'For POST request...'

@app.route('/list_files', methods=['GET'])
def list_files():
    return get_files_json()

def get_files_json():
    # path to directory of joints data.
    directory_path = './mediapipe'
    
    # Get data as txt.
    files = [f for f in os.listdir(directory_path) if f.endswith('.txt')]
    
    return flask.jsonify({'files': files})

@app.route('/download_animation/<string:file>', methods=['GET'])
def download_animation(file):
    return flask.send_from_directory('../mediapipe', file, as_attachment=True)

if __name__ == '__main__':
    app.run(host='192.168.50.110', port=8000)