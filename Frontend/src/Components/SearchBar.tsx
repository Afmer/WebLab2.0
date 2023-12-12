import { ChangeEvent, useRef, useState } from 'react';
import '../CSS/SearchBar.css'
import axios from 'axios';
import { Link } from 'react-router-dom';

interface SearchShowResult {
    showId: string,
    name: string
}
const SearchBar = () => {
    const [isFocusOnSearchBar, setIsFocusOnSearchBar] = useState(false);
    const [isMouseLeave, setIsMouseLeave] = useState(true);
    const isShowResult = () => isFocusOnSearchBar || !isMouseLeave;
    const [isLoading, setIsLoading] = useState(false);
    const [inputText, setInputText] = useState('');
    const [searchResult, setSearchResult] = useState<SearchShowResult[]>([]);
    const timerRef = useRef<NodeJS.Timeout | null>(null);
    const getSearchResult = (query: string) => {
        axios.get(`/api/Search/Shows?query=${query}`)
            .then(response => {
                const data = response.data as SearchShowResult[]
                setSearchResult(data)
                setIsLoading(false)
            })
            .catch(error => {
                // Обработка ошибок
                console.error(error);
            });
    }
    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const text = event.target.value
        setInputText(text);
        if(text === null || text === "")
        {
            setSearchResult([])
            return;
        }
        if (timerRef.current) {
            clearTimeout(timerRef.current);
        }
        setIsLoading(true)
        timerRef.current = setTimeout(() => getSearchResult(inputText), 1000)
    }
    const onSearchFocus = (event: ChangeEvent<HTMLInputElement>) => {
        setIsFocusOnSearchBar(true);
    }
    const onSearchBlur = (event: ChangeEvent<HTMLInputElement>) => {
        setIsFocusOnSearchBar(false);
    }
    const handleMouseLeave = () => {
        setIsMouseLeave(true)
    };
    const handleMouseEnter = () => {
        setIsMouseLeave(false)
    };
    return (
        <div className="search-container">
            <div>
            <div>
                <input
                    type="text"
                    placeholder="Поиск спектаклей..."
                    onFocus={onSearchFocus}
                    onBlur={onSearchBlur}
                    onChange={handleInputChange}
                    value={inputText}
                />
            </div>
            {isShowResult() &&(
            <div className="search-results-container" onMouseEnter={handleMouseEnter} onMouseLeave={handleMouseLeave}>
                {!isLoading ? 
                (<ul className="search-results">
                    {searchResult.map((item, index) => (
                        <li key={index}><Link to={`/Show/${item.showId}`}>{item.name}</Link></li>
                    ))}
                </ul>)
                : (<p>Загрузка</p>)}
            </div>)}
            </div>
        </div>
    )
}

export default SearchBar