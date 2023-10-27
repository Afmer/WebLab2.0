import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import '../CSS/Show.css'
interface ShowData {
    id: string,
    name: string,
    description: string,
    date: Date
    labelImage: string
}
const Show: React.FC = () => {
    const [show, setShow] = useState<ShowData>();
    const params = useParams();
    useEffect(() => {
        var http = `/api/Shows/Show?id=${params.id}`;
        const response =axios.get(http)
            .then(response => {
                var data = response.data as ShowData
                setShow({
                    id: data.id,
                    name: data.name,
                    description: data.description,
                    date: new Date(data.date),
                    labelImage: data.labelImage
                });
            })
            .catch(error => {
                console.error('Произошла ошибка:', error);
            });
        
    }, []);
    return (
        <div className='show'>
            <table>
                <tr>
                    <td className='left-cell'>
                        <div className='background' style={{backgroundImage: `url("/api/Image/Show?id=${show?.labelImage}&imageArea=Shows`}}>
                            <p className='name'>{show?.name}</p>
                            <p className='date'>{show?.date.toLocaleDateString()}</p>
                        </div>
                    </td>
                    <td className='right-cell'>
                        <div className='background'>
                            <div className='description'>
                                <p>{show?.description}</p>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    )
}

export default Show