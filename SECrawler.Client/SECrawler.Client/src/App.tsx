import { useEffect, useState } from 'react';
import './App.css';
import {
    Button,
    Input,
    Space,
    Table,
    Typography,
    Spin,
    } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type {
    ApiResult,
    SearchEngineItem,
    SearchResultHistoriesItem,
    HistoriesDataType,
} from './Types';
import UseFetch from './Hooks/UseFetch';

const App = () => {
    const baseUrl ="https://localhost:7292/";
    const [searchEngineList, setSearchEngineList] = useState<SearchEngineItem[]>([]);
    const [searchResultList, setSearchResultList] = useState<SearchResultHistoriesItem[]>([]);
    const [selectedSearchEngine, setSelectedSearchEngine] = useState<SearchEngineItem>();
    const [searchKey, setSearchKey] = useState<string>('');
    const [searchResult, setSearchResult] = useState<string>('');
    const [rawRankings, setRawRankings] = useState<number[]>([]);


    const [isSearching, setIsSearching] = useState(false);
    const [isSaving, setIsSaving] = useState(false);
    const [isLoadingEngines, setIsLoadingEngines] = useState(true);
    const [isLoadingHistory, setIsLoadingHistory] = useState(true);

    const { Title, Text } = Typography;

    const {
        data: enginesResult,
        fetchData: fetchEngines,
    } = UseFetch<ApiResult<SearchEngineItem[]>>(
        `${baseUrl}api/Engine/SearchEngines`, 'GET', false);

    const {
        data: searchedData,
        fetchData: search,
    } = UseFetch<ApiResult<number[]>>(
        `${baseUrl}api/Engine/Search?Query=${searchKey}&PageSize=100&EngineId=${selectedSearchEngine?.id ?? 0}`,
        'GET',
        false
    );

    const {
        data: historyResult,
        fetchData: fetchHistory,
    } = UseFetch<ApiResult<SearchResultHistoriesItem[]>>(
        `${baseUrl}api/SearchResult/Histories`,
        'GET',
        false
    );

    const {
        fetchData: saveSearchResult
    } = UseFetch<ApiResult<boolean>>(
        `${baseUrl}api/SearchResult/SaveSearchHistory`,
        'POST',
        true
    );

    useEffect(() => {
        (async () => {
            setIsLoadingEngines(true);
            await fetchEngines();
            setIsLoadingEngines(false);

            setIsLoadingHistory(true);
            await fetchHistory();
            setIsLoadingHistory(false);
        })();
    }, []);

    useEffect(() => {
        if (enginesResult) {
            setSearchEngineList(enginesResult.data ?? []);
        }
    }, [enginesResult]);

    useEffect(() => {
        if (historyResult) {
            setSearchResultList(historyResult.data ?? []);
        }
    }, [historyResult]);

    useEffect(() => {
        if (searchedData && Array.isArray(searchedData.data)) {
            setRawRankings(searchedData.data);
            setSearchResult(searchedData.data.length > 0
                ? searchedData.data.join(', ')
                : 'Not found');
        }
    }, [searchedData]);

    const searchApi = async () => {
        if (!searchKey.trim()) {
           alert('Please enter a search keyword.');
            return;
        }

        if (!selectedSearchEngine) {
            alert('Please select a search engine.');
            return;
        }

        setIsSearching(true);
        await search();
        setIsSearching(false);
    };


    const saveResult = async () => {
        if (!searchKey || !selectedSearchEngine || rawRankings.length === 0) {
           alert('Please perform a search before saving.');
            return;
        }

        setIsSaving(true);
        await saveSearchResult({
            body: {
                keyWord: searchKey,
                engineId: selectedSearchEngine.id,
                rankings: rawRankings,
            }
        });

        await fetchHistory();
        setIsSaving(false);
    };

    const columns: ColumnsType<HistoriesDataType> = [
        { title: 'Id', dataIndex: 'id', key: 'id' },
        { title: 'Rank', dataIndex: 'rank', key: 'rank' },
        { title: 'URL', dataIndex: 'engineType', key: 'engineType' },
        { title: 'KeyWords', dataIndex: 'keyWords', key: 'keyWords' },
        { title: 'Date', dataIndex: 'date', key: 'date' },
    ];

    return (
        <div className="app-container">
            <Title>Search Engine Ranking Tool</Title>

            <Title level={4}>Select a Search Engine</Title>
            <Spin spinning={isLoadingEngines}>
                <div className="engine-button-group">
                    <Space wrap>
                        {searchEngineList.map((mapItem) => (
                            <Button
                                key={mapItem.id}
                                onClick={() => {
                                    setSearchEngineList(
                                        searchEngineList.map((item) => ({
                                            ...item,
                                            select: item.id === mapItem.id,
                                        }))
                                    );
                                    setSelectedSearchEngine(mapItem);
                                }}
                                type={mapItem.id === selectedSearchEngine?.id ? 'primary' : 'default'}
                            >
                                {mapItem.name}
                            </Button>
                        ))}
                    </Space>
                </div>
            </Spin>

            <div className="search-bar">
                <Input.Group compact>
                   
                    <Input
                        className="search-input"
                        value={searchKey}
                        onChange={(e) => setSearchKey(e.target.value)}
                        placeholder="Search keywords"
                    />
                   
                   
                </Input.Group>
                <Button className={"control-button"} onClick={searchApi} type="primary" loading={isSearching}>
                    Search
                </Button>
                <Button className={"control-button"} onClick={saveResult} loading={isSaving}>
                    Save & Refresh History
                </Button>
                {searchResult && (
                    <div className="ranking-display">
                        <Text strong>Rankings:</Text>
                        <div className="rank-box">
                            {searchResult}
                        </div>
                    </div>
                )}
            </div>

            <Title level={4}>Search History</Title>
            <Spin spinning={isLoadingHistory}>
                <Table
                    className="history-table"
                    rowKey="id"
                    columns={columns}
                    dataSource={searchResultList}
                    pagination={{ pageSize: 5 }}
                />
            </Spin>
        </div>
    );
};

export default App;
