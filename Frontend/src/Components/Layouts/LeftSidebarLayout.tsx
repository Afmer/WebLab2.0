import React, { useState } from 'react';
import '../../CSS/LeftSidebarLayout.css';
const LeftSidebarLayout: React.FC = (props) => {
    const [isOpen, setIsOpen] = useState(false);

    const toggleSidebar = () => {
        setIsOpen(!isOpen);
    };
    return (
        <div>
            <div className={`sidebar ${isOpen ? 'open' : ''}`}>
                <button className='toggle-button' onClick={toggleSidebar}></button>
                <ul>
                    <li>Item 1</li>
                    <li>Item 2</li>
                    <li>Item 3</li>
                </ul>
            </div>l
        </div>
    );
}

export default LeftSidebarLayout;
