from __future__ import annotations

import sys
from genetic_algorithm import *


class OneMax(BinaryGene):
    """OneMax問題用にBinaryGeneを継承したクラス。
    """
    @property
    def number(self) -> float:
        """遺伝子情報を十進数に変換した値を取得するプロパティ。

        Returns:
            float: 遺伝子情報を十進数に変換した値。(0.0～1.0)
        """
        table = [2 ** i for i in reversed(range(self._length))]
        return sum([v * t for v, t in zip(self._value, table)]) / sum(table)

    @property
    def evaluation(self) -> float:
        """遺伝子情報からOneMax問題の解を取得するプロパティ。

        Returns:
            float: 遺伝子情報の評価値。
        """
        return self.number


if __name__ == '__main__':
    ga = GeneticAlgorithm(OneMax)
    try:
        from matplotlib import pyplot as plt

        def plot(genetic_algorithm: GeneticAlgorithm):
            """OneMax問題をmatplotlibを使用してプロットするメソッド。

            Args:
                genetic_algorithm (GeneticAlgorithm): プロットする遺伝的アルゴリズム。
            """
            fig, ax = plt.subplots(1, 1)
            fig.canvas.mpl_connect('close_event', sys.exit)
            x = [_x * 0.001 for _x in range(0, 1001)]
            ax.plot(x, x)
            x = [population.number for population in genetic_algorithm.population]
            y = [population.evaluation for population in genetic_algorithm.population]
            point, = ax.plot(x, y, linestyle='None', marker='o')
            while True:
                x = [population.number for population in genetic_algorithm.population]
                y = [population.evaluation for population in genetic_algorithm.population]
                point.set_data(x, y)
                genetic_algorithm.run(is_output=False)
                plt.pause(.01)

        plot(ga)
    except ImportError:
        ga.run()
