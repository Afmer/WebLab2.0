import React, { useState, useEffect } from 'react';
interface PartViewData {
    html: string;
  }
function Home() {
    const [htmlObj, setHtmlObj] = useState<PartViewData | null>(null);
    useEffect(() => {
        fetch('/api/Home')  // Путь начинается с '/'
        .then(response => {
            if (!response.ok) {
            throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then((data : PartViewData) => {
            setHtmlObj(data);
        })
        .catch(error => {
            console.error('Error:', error);
        });
    }, []);
    return (
        <div>
            <div dangerouslySetInnerHTML={{ __html: htmlObj ? htmlObj.html : "" }} />
        </div>
    )
}

export default Home