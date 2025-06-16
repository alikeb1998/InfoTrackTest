import { describe, it, vi, expect, beforeEach } from 'vitest';
import { render, screen } from '@testing-library/react';
import App from '../src/App';


vi.mock('./Hooks/UseFetch', () => {
    return {
        default: () => ({
            data: null,
            fetchData: vi.fn(),
        }),
    };
});



beforeEach(() => {
    Object.defineProperty(window, 'matchMedia', {
        writable: true,
        value: vi.fn().mockImplementation((query) => ({
            matches: false,
            media: query,
            onchange: null,
            addListener: vi.fn(),
            removeListener: vi.fn(),
            addEventListener: vi.fn(),
            removeEventListener: vi.fn(),
            dispatchEvent: vi.fn(),
        })),
    });
});


describe('App Component', () => {
    it('renders main title', () => {
        render(<App />);
        expect(screen.getByText('Search Engine Ranking Tool')).toBeInTheDocument();
    });

    it('renders search input', () => {
        render(<App />);
        expect(screen.getByPlaceholderText('Search keywords')).toBeInTheDocument();
    });

    it('renders Search button', () => {
        render(<App />);
        expect(screen.getByRole('button', { name: /search/i })).toBeInTheDocument();
    });

    it('renders Save button', () => {
        render(<App />);
        expect(screen.getByRole('button', { name: /save & refresh history/i })).toBeInTheDocument();
    });
});
