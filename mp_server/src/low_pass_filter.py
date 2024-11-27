import numpy as np              # type: ignore
import matplotlib.pyplot as plt # type: ignore
import ast

# Written based on
# https://qiita.com/ajiron/items/ca630de8b6e3ed28ad1e
def low_pass_filter(sequence):
    sequence = np.array(sequence)
    sequence = np.transpose(sequence, axes=(1, 2, 0))

    # Parameter of data
    N = sequence.shape[2]   # sample num

    # axis
    t = np.arange(0, N)
    freq = np.linspace(0, 1.0, N) # 周波数軸

    fc = 0.1            # カットオフ周波数
    fs = 1              # サンプリング周波数
    fm = (1/2) * fs     # アンチエリアジング周波数
    fc_upper = fs - fc  # 上側のカットオフ　fc～fc_upperの部分をカット

    f = sequence

    # 元波形をfft
    F = np.fft.fft(f)

    # 元波形をコピーする
    G = F.copy()
    
    # ローパス
    for i in range(G.shape[0]):
        for j in range(G.shape[1]):
            G[i][j][((freq > fc)&(freq< fc_upper))] = 0 + 0j

    # 高速逆フーリエ変換
    g = np.fft.ifft(G)

    # 実部の値のみ取り出し
    g = g.real

    # プロット確認
    # plt.subplot(221)
    # plt.plot(t, f[25][1])

    # plt.subplot(222)
    # plt.plot(freq, F[25][1])

    # plt.subplot(223)
    # plt.plot(t, g[25][1])

    # plt.subplot(224)
    # plt.plot(freq, G[25][1])
    
    # plt.show()

    g = np.transpose(g, axes=(2, 0, 1))
    g_str = [[tuple(row) for row in matrix] for matrix in g]
    return g_str

if __name__ == "__main__":
    file_path = "mediapipe/video5.txt"

    with open(file_path, "r") as file:
        data = file.read()

    # Read str as python object.
    array_data = ast.literal_eval(data)

    low_pass_filter(array_data)