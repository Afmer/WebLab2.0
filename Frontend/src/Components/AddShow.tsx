import React, { ChangeEvent, FormEvent, useState } from 'react';
import ImageSelector from './ImageSelector';
import '../CSS/AddShow.css'
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

interface Form {
    name: string,
    date: Date,
    description: string,
    labelImage: File | null,
    images: File[] | null
  }
function AddShow() {
    const navigate = useNavigate();
    const [formData, setFormData] = useState<Form>({
        name: "",
        date: new Date(),
        description: "",
        labelImage: null,
        images: null
    });
    const handleChange = (e: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        console.log(value)
        setFormData({
          ...formData,
          [name]: value
        });
      };
    const handleShowsImagesChange = (updatedImages: File[]) => {
        console.log(updatedImages);
        setFormData({
            ...formData,
            images: updatedImages
        });
    }
    const handleLabelImagesChange = (e: ChangeEvent<HTMLInputElement>) => {
        const value = e.target.files![0]
        console.log(value)
        setFormData({
          ...formData,
          labelImage: value
        });
    }
    const handleSubmit = (event: FormEvent<HTMLFormElement>): void => {
        event.preventDefault();
        if(formData.labelImage === null)
            return
        const formForUpload = new FormData();
        formForUpload.append('name', formData.name);
        formForUpload.append('date', formData.date.toString());
        formForUpload.append('description', formData.description);
        formForUpload.append('labelImage', formData.labelImage);
        if(formData.images !== null)
            for(let i = 0; i < formData.images.length; i++)
                formForUpload.append(`images`, formData.images[i])
        const config = {
            headers: {
              'Content-Type': 'multipart/form-data'
            }
          };
        axios.post('api/Shows/Add', formForUpload, config)
          .then(response => {
            navigate(`/Show/${response.data.showId}`);
          })
          .catch((error) => {
            console.error('Ошибка:', error.config);
          });
    }

    return (
        <div className='add-show'>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="name">Имя пользователя</label>
                    <input type="text" id='name'  name='name' value={formData.name} onChange={handleChange}/>
                </div>
                <div>
                    <label htmlFor="date">Дата проведения</label>
                    <input type="date" id='date' name='date' value={formData.date.toString()} onChange={handleChange}/>
                </div>
                <div>
                    <label htmlFor="description">Описание</label>
                    <textarea id='description' name='description' value={formData.description} onChange={handleChange}/>
                </div>
                <div>
                    <label htmlFor="labelImage">Обложка</label>
                    <input type='file' id='labelImage' name='labelImage' onChange={handleLabelImagesChange}/>
                </div>
                <div>
                    <ImageSelector onUpdate={handleShowsImagesChange}/>
                </div>
                <div>
                    <input type="submit" />
                </div>
            </form>
        </div>
    )
}

export default AddShow