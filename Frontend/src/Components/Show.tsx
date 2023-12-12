import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import '../CSS/Show.css'
import ImageGallery from "react-image-gallery";
import "react-image-gallery/styles/css/image-gallery.css";
interface ShowData {
    id: string,
    name: string,
    description: string,
    date: Date
    labelImage: string
    images: string[]
}
const Show: React.FC = () => {
    const [show, setShow] = useState<ShowData>();
    const params = useParams();
    const images = [
        {
            original: "/api/Image/Show?id=da00da3a-ee18-40bf-bd68-2cbd53d41b7d&imageArea=Shows",
            thumbnail: "/api/Image/Show?id=da00da3a-ee18-40bf-bd68-2cbd53d41b7d&imageArea=Shows"
        },
        {
            original: "/api/Image/Show?id=e9aab09d-4cb6-4d13-808d-671fff8e701c&imageArea=Shows",
            thumbnail: "/api/Image/Show?id=e9aab09d-4cb6-4d13-808d-671fff8e701c&imageArea=Shows"
        },
        {
            original: "/api/Image/Show?id=700298ae-4d4e-4075-9ae5-cc3220bb9259&imageArea=Shows",
            thumbnail: "/api/Image/Show?id=700298ae-4d4e-4075-9ae5-cc3220bb9259&imageArea=Shows"
        }
    ]
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
                    labelImage: data.labelImage,
                    images: data.images
                });
            })
            .catch(error => {
                console.error('Произошла ошибка:', error);
            });
        
    }, [params]);
    const getImages  = () => {
        if(show !== undefined && show.images !== undefined)
        {
            var result = show.images.map(x => {
                return {
                    original: `/api/Image/Show?id=${x}&imageArea=ShowsExtraImages`,
                    thumbnail: `/api/Image/Show?id=${x}&imageArea=ShowsExtraImages`
                }
            });
            return result
        }
        else
            return []
    }
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
                <tr>
                    <td colSpan={2}>
                        <div className='background background-slider'>
                            <ImageGallery items={getImages()} showPlayButton={false} additionalClass='slider'/>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    )
}

export default Show