from __future__ import annotations

import sys
from genetic_algorithm import *


class QuadraticFunction(BinaryGene):
    """二次関数用にBinaryGeneを継承したクラス。
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
        """遺伝子情報から二次関数の解を取得するプロパティ。

        Returns:
            float: 遺伝子情報の評価値。
        """
        return (self.number) ** 2


if __name__ == '__main__':
    ga = GeneticAlgorithm(QuadraticFunction, crossover_type=CrossoverType.UNIFORM)
    try:
        from matplotlib import pyplot as plt

        def plot(genetic_algorithm: GeneticAlgorithm):
            """二次関数をmatplotlibを使用してプロットするメソッド。

            Args:
                genetic_algorithm (GeneticAlgorithm): プロットする遺伝的アルゴリズム。
            """
            fig, ax = plt.subplots(1, 1)
            fig.canvas.mpl_connect('close_event', sys.exit)
            x = [_x * 0.001 for _x in range(0, 1001)]
            y = [(_x) ** 2 for _x in x]
            ax.plot(x, y)
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
