import { useState } from 'react';
import './App.css';

function App() {
    const [number, setNumber] = useState('');
    const [words, setWords] = useState('');
    const [error, setError] = useState(''); // State to store error messages

    const convertToWords = async () => {

        try {
            const response = await fetch(`https://localhost:7134/NumberToWords/${number}`);
            const data = await response.text();

            if (!response.ok) {
                throw new Error(data || 'Invalid input data');
            }

            setWords(data); 
            setError('');
        } catch (error) {
            console.error('Error fetching data:', error);
            setError(error.message); 
            setWords('');
        }
    };

    return (
        <div className="center-container">
            <h1>Number to Words Converter</h1>
            <p>Please enter a number to convert it to words:</p>
            <input
                type="text"
                value={number}
                onChange={(e) => setNumber(e.target.value)}
                placeholder="Enter a number"
                className="input-field"
            />
            <button
                onClick={convertToWords}
                className="submit-button">
                Convert
            </button>
            {words && <p>Result: {words}</p>}
            {error && <p style={{ color: 'red' }}>Error: {error}</p>}
        </div>
    );
}

export default App;