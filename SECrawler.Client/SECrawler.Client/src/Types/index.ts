export interface SearchEngineItem {
    id: number;
    name: string;
    select?: boolean;
}

export interface SearchResultHistoriesItem {
    id: number;
    engineType: string;
    keyWords: string;
    rank: string;
    date: string;
}

export interface ApiResult<T>{
    data: T;
    isSuccessful: boolean;
    errorMessage: string;
}

export interface HistoriesDataType {
    engineType: string;
    keyWords: string;
    rank: string;
    date: string;
    id: number;
}