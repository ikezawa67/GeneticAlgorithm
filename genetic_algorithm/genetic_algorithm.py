"""遺伝的アルゴリズム"""
from __future__ import annotations

import warnings
from copy import deepcopy
from enum import Enum, auto
from random import choices, random, randint, sample
from abc import ABCMeta, abstractmethod


class SelectionType(Enum):
    """選択方式の列挙型定数。
    """
    ROULETTE = auto()  # ルーレット選択
    RANKING = auto()  # ランキング選択
    TOURNAMENT = auto()  # トーナメント選択


class CrossoverType(Enum):
    """交差方式の列挙型定数。
    """
    SINGLE_POINT = auto()  # 一点交叉
    TWO_POINT = auto()  # 二点交叉
    UNIFORM = auto()  # 一様交叉


class BinaryGene(metaclass=ABCMeta):
    """二進数で表現された遺伝子情報型抽象クラス。
    """

    def __init__(self, length: int, crossover_type: CrossoverType, initial_value: list[int] | None = None) -> None:
        """コンストラクタ

        Args:
            length (int): 遺伝子情報の長さ。
            crossover_type (CrossoverType): 交差方式。
            initial_value (list[int] | None, optional): 初期値の二進で表現された値が格納されたリスト。デフォルトはNone。
        """
        self._length = length
        self._crossover_type = crossover_type
        if initial_value is None:
            self._value = [randint(0, 1) for _ in range(self._length)]
        elif self._length != len(initial_value):
            warnings.warn('渡された遺伝子情報の長さが異なるためランダムに遺伝子情報を生成しました。')
            self._value = [randint(0, 1) for _ in range(self._length)]
        elif not all([v in [0, 1] for v in initial_value]):
            warnings.warn('渡された遺伝子情報が二進数リストでないためランダムに遺伝子情報を生成しました。')
            self._value = [randint(0, 1) for _ in range(self._length)]
        else:
            self._value = initial_value

    def __repr__(self) -> str:
        """クラスの文字列表現を返すメソッド。

        Returns:
            str: クラスの文字列表現。
        """
        return f'number: {self.number}, evaluation: {self.evaluation}'

    @property
    @abstractmethod
    def number(self) -> tuple[float, ...]:
        """遺伝子情報を十進数に変換した値を取得する抽象プロパティ。

        Returns:
            tuple[float, ...]: 遺伝子情報を十進数に変換した値。
        """

    @property
    @abstractmethod
    def evaluation(self) -> float:
        """遺伝子情報の評価値を取得する抽象プロパティ。

        Returns:
            float: 遺伝子情報の評価値。
        """

    def mutation(self) -> None:
        """遺伝子情報を突然変異させるメソッド。
        """
        self._value = list(map(lambda v: int(not v), self._value))

    def crossover(self, other: BinaryGene) -> tuple[BinaryGene, BinaryGene]:
        """遺伝子情報を交叉させるメソッド。

        Args:
            other (BinaryGene): 交叉で参照する遺伝子情報。

        Raises:
            ValueError: 対応していない交差方式が指定された場合。

        Returns:
            tuple[BinaryGene, BinaryGene]: 交叉した後に生成された2つの遺伝子情報を格納したタプル。
        """
        child_0 = deepcopy(self)
        child_1 = deepcopy(other)
        if self._crossover_type == CrossoverType.SINGLE_POINT:
            point = randint(0, self._length)
            tmp = child_0[:point]
            child_0[:point] = child_1[:point]
            child_1[:point] = tmp
        elif self._crossover_type == CrossoverType.TWO_POINT:
            points = sorted(sample(range(self._length), 2))
            tmp = child_0[points[0]:points[1]]
            child_0[points[0]:points[1]] = child_1[points[0]:points[1]]
            child_1[points[0]:points[1]] = tmp
        elif self._crossover_type == CrossoverType.UNIFORM:
            for i in range(self._length):
                if random() < 0.5:
                    tmp = child_0[i]
                    child_0[i] = child_1[i]
                    child_1[i] = child_0[i]
        else:
            raise ValueError(f'対応していない交差方式が指定されています : {self._crossover_type}')
        return tuple([child_0, child_1])

    def __lt__(self, other: BinaryGene) -> bool:
        """遺伝子情報の評価値の小なり比較用メソッド。

        Args:
            other (BinaryGene): 比較で参照する遺伝子情報。

        Returns:
            bool: 小なり比較条件を満たすかどうかの真偽値。
        """
        return self.evaluation < other.evaluation

    def __le__(self, other: BinaryGene) -> bool:
        """遺伝子情報の評価値の以下比較用メソッド。

        Args:
            other (BinaryGene): 比較で参照する遺伝子情報。

        Returns:
            bool: 以下比較条件を満たすかどうかの真偽値。
        """
        return self.evaluation <= other.evaluation

    def __eq__(self, other: BinaryGene) -> bool:
        """遺伝子情報の評価値の等値比較用メソッド。

        Args:
            other (BinaryGene): 比較で参照する遺伝子情報。

        Returns:
            bool: 等値比較条件を満たすかどうかの真偽値。
        """
        return self.evaluation == other.evaluation

    def __ne__(self, other: BinaryGene) -> bool:
        """遺伝子情報の評価値の非等値比較用メソッド。

        Args:
            other (BinaryGene): 比較で参照する遺伝子情報。

        Returns:
            bool: 非等値比較条件を満たすかどうかの真偽値。
        """
        return self.evaluation != other.evaluation

    def __ge__(self, other: BinaryGene) -> bool:
        """遺伝子情報の評価値の以上比較用メソッド。

        Args:
            other (BinaryGene): 比較で参照する遺伝子情報。

        Returns:
            bool: 以上比較条件を満たすかどうかの真偽値。
        """
        return self.evaluation >= other.evaluation

    def __gt__(self, other: BinaryGene) -> bool:
        """遺伝子情報の評価値の大なり比較用メソッド。

        Args:
            other (BinaryGene): 比較で参照する遺伝子情報。

        Returns:
            bool: 大なり比較条件を満たすかどうかの真偽値。
        """
        return self.evaluation > other.evaluation

    def __getitem__(self, key: int | slice) -> int | list[int]:
        """要素の値を参照するメソッド。

        Args:
            key (int | slice): インデックスまたは、スライス。

        Returns:
            int | list[int]: 参照した遺伝子情報の値。
        """
        return self._value[key]

    def __setitem__(self, key: int | slice, value: int | list[int]) -> None:
        """要素に値を代入するメソッド。

        Args:
            key (int | slice): インデックスまたは、スライス。
            value (int | list[int]): 代入する遺伝子情報の値。

        Raises:
            ValueError: 渡された遺伝子情報が二進数でない場合。
        """
        if isinstance(value, int) and value in [0, 1]:
            self._value[key] = value
        elif all([v in [0, 1] for v in value]):
            self._value[key] = value
        else:
            raise ValueError('渡された遺伝子情報が二進数ではありません。')


class GeneticAlgorithm:
    """遺伝的アルゴリズムを扱うクラス。
    """

    def __init__(self, gene_class: type[BinaryGene], gene_length: int = 20, gene_population_size: int = 100, mutation_probability: float = 0.1, crossover_probability: float = 0.01, number_elites: int = 5, selection_type: SelectionType = SelectionType.ROULETTE, crossover_type: CrossoverType = CrossoverType.SINGLE_POINT) -> None:
        """コンストラクタ

        Args:
            gene_class (type[BinaryGene]): 遺伝子情報クラス。
            gene_length (int, optional): 遺伝子情報の長さ。デフォルト8。
            gene_population_size (int, optional): 遺伝子情報のリストのサイズ。デフォルト50。
            mutation_probability (float, optional): 変異確率(0.0～1.0)。デフォルト0.1。
            crossover_probability (float, optional): 交叉確率(0.0～1.0)。デフォルト0.01。
            number_elites (int, optional): 次世代に残す評価値の高い遺伝子情報の個数。デフォルトは5。
            selection_type (SelectionType, optional): 選択方式。デフォルトSelectionType.ROULETTE。
            crossover_type (CrossoverType, optional): 交差方式。デフォルトCrossoverType.SINGLE_POINT。
        """
        self._population: list[BinaryGene] = [gene_class(gene_length, crossover_type) for _ in range(gene_population_size)]
        self._mutation_probability: float = mutation_probability
        self._crossover_probability: float = crossover_probability
        self._number_elites: int = number_elites
        self._selection_type: SelectionType = selection_type
        self.number_of_generations = 0

    @property
    def population(self) -> list[BinaryGene]:
        """遺伝子情報のリストを取得するプロパティ

        Returns:
            list[BinaryGene]: 遺伝子情報のリスト
        """
        return self._population

    @property
    def best_gene(self) -> BinaryGene:
        """遺伝子情報のリストから、評価値が最も高い遺伝子情報を取得するプロパティ。

        Returns:
            BinaryGene: 遺伝子情報の評価値が最も高い遺伝子情報。
        """
        return max(self._population)

    def _elites_selection(self) -> list[BinaryGene]:
        elites = sorted(self._population, reverse=True)
        return deepcopy(elites[:self._number_elites])

    def _roulette_selection(self) -> tuple[BinaryGene, BinaryGene]:
        """ルーレット選択を行い、交叉等で利用する2つの遺伝子情報を取得するメソッド。

        Returns:
            tuple[BinaryGene, BinaryGene]: 選択された2つの遺伝子情報を格納したタプル。
        """
        min_evaluation = min(self._population).evaluation
        max_evaluation = max(self._population).evaluation
        try:
            weights = [(population.evaluation - min_evaluation) / (max_evaluation - min_evaluation) for population in self._population]
        except ZeroDivisionError:
            return tuple(choices(self._population, k=2))
        return tuple(choices(self._population, weights=weights, k=2))

    def ranking_selection(self) -> tuple[BinaryGene, BinaryGene]:
        """ランキング選択を行い、交叉等で利用する2つの遺伝子情報を取得するメソッド。

        Returns:
            tuple[BinaryGene, BinaryGene]: 選択された2つの遺伝子情報を格納したタプル。
        """
        rankings = sorted(self._population)
        weights = [i for i, _ in enumerate(rankings, 1)]
        min_weight = min(weights)
        max_weight = max(weights)
        weights = [(weight - min_weight) / (max_weight - min_weight) for weight in weights]
        return tuple(choices(rankings, weights=weights, k=2))

    def _tournament_selection(self) -> tuple[BinaryGene, BinaryGene]:
        """トーナメント選択を行い、交叉等で利用する2つの遺伝子情報を取得するメソッド。

        Returns:
            tuple[BinaryGene, BinaryGene]: 選択された2つの遺伝子情報を格納したタプル。
        """
        participants = choices(self._population, k=len(self._population) // 2)
        return tuple(sorted(participants, reverse=True)[:2])

    def _get_parents(self) -> tuple[BinaryGene, BinaryGene]:
        """選択方式に応じた親の2つの遺伝子情報のタプルを取得するメソッド。

        Raises:
            ValueError: 対応していない選択方式が指定された場合。

        Returns:
            tuple[BinaryGene, BinaryGene]: 取得された親の2つの個体遺伝子情報のタプル。
        """
        if self._selection_type == SelectionType.ROULETTE:
            return self._roulette_selection()
        elif self._selection_type == SelectionType.RANKING:
            return self.ranking_selection()
        elif self._selection_type == SelectionType.TOURNAMENT:
            return self._tournament_selection()
        else:
            raise ValueError(f'対応していない選択方式が指定されています : {self._selection_type}')

    def _get_next_genes(self, parents: tuple[BinaryGene, BinaryGene]) -> tuple[BinaryGene, BinaryGene]:
        """親の2つの遺伝子情報のタプルトから、特定の確率で交差、突然変異を行った次世代の2つの遺伝子情報のタプルを取得するメソッド。

        Args:
            parents (tuple[BinaryGene, BinaryGene]): 親の2つの遺伝子情報を格納したタプル。

        Returns:
            tuple[BinaryGene, BinaryGene]: 次世代の2つの遺伝子情報を格納したタプル。
        """
        next_genes = parents
        if random() < self._crossover_probability:
            next_genes = parents[0].crossover(parents[1])
        if random() < self._mutation_probability:
            for gene in next_genes:
                gene.mutation()
        return next_genes

    def _replacement_next_generation(self) -> None:
        """次世代の遺伝子情報を生成し、遺伝子情報を生成した次世代の遺伝子情報で置換するメソッド。
        """
        new_population = self._elites_selection()
        while len(new_population) < len(self._population):
            parents = self._get_parents()
            next_genes = self._get_next_genes(parents)
            new_population.extend(next_genes)
        while len(new_population) > len(self._population):
            del new_population[-1]
        self._population = new_population

    def run(self, run_generation: int = 1, is_output: bool = True) -> None:
        """遺伝的アルゴリズムを実行するメソッド。

        Args:
            run_generation (int, optional): 計算する世代数。デフォルトは1。
            is_output (bool, optional): 最良個体の情報を出力するかの真理値。デフォルトはTrue。
        """
        for _ in range(run_generation):
            if is_output:
                print(f'世代数 : {self.number_of_generations}, 最良個体 : {self.best_gene}')
            self._replacement_next_generation()
            self.number_of_generations += 1
