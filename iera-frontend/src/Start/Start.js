//Generic Components go here
// E.G. buttons and other shit of that kind
import React from 'react';
import { useHistory } from 'react-router-dom'

const LinkButton = ({to}) => {
    const history= useHistory();

    const handleClick = () => {
        history.push(to);
    };

    return (
        <button onClick={handleClick}>
            <p>Im a buttin</p>
        </button>
    )
}