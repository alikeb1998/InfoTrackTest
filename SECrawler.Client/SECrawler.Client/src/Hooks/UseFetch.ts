import { useState, useCallback } from "react";


type FetchResponse<T> = {
    data: T | null;
    error: string | null;
    isLoading: boolean;
};

const useFetch = <T>(
    url: string,
    method: "GET" | "POST",
    passBodyAsJson?: boolean
) => {
    const [response, setResponse] = useState<FetchResponse<T>>({
        data: null,
        error: null,
        isLoading: false,
    });

    const fetchData = useCallback(
        async ({
                   body,
               }: { body?: any; } = {}) => {
            setResponse((prev) => ({ ...prev, isLoading: true }));

            try {
                const headers: Record<string, string> = {
                    ...(passBodyAsJson ? { "Content-Type": "application/json" } : {}),
                };

                const request: RequestInit = {
                    method,
                    body: method === "POST" && body ? (passBodyAsJson ? JSON.stringify(body) : body) : null,
                    headers,
                };

                const response = await fetch(url, request);

                if (!response.ok) {
                    throw new Error(`Failed to fetch, status: ${response.status}`);
                }

                const data: T = await response.json();
                setResponse({ data, error: null, isLoading: false });
            } catch (error: any) {
                setResponse({ data: null, error: error.message, isLoading: false });
            }
        },
        [url, method, passBodyAsJson]
    );


    return { ...response, fetchData };
};

export default useFetch;