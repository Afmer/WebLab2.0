import '../CSS/ImageSelector.css'
import React, { DragEventHandler, useState } from 'react';

interface Props
{
    onUpdate?: (images: File[]) => void
}
const ImageSelector: React.FC<Props> = ({onUpdate})  => {
    const [images, setImages] = useState<File[]>([])
    const AddImages = (files: File[]) => {
        for(let i = 0; i < files.length; i++)
        {
            const file = files[i]
            const reader = new FileReader();
            const item = document.createElement('div');
            item.className = 'item';
            const itemIcon = document.createElement('img');
            itemIcon.className = 'item-icon';
            reader.onload = function (event) {
              itemIcon.src = event.target!.result as string;
            };
            reader.readAsDataURL(file);
            item.appendChild(itemIcon);
            const itemName = document.createElement('p');
            itemName.textContent = file.name;
            itemName.className = 'item-name'
            item.appendChild(itemName);
            const dropArea = document.getElementById('image-selector-container');
            if (dropArea) {
                dropArea.appendChild(item);
            }
        }
        const newImages = [...images, ...files];
        setImages(newImages);
        onUpdate?.(newImages);
    };
    const handleDragOver = (e: React.DragEvent<HTMLElement>) => {
        e.preventDefault();
        e.dataTransfer.dropEffect = 'copy';
        e.currentTarget.style.cursor = 'copy'; 
      };
    const handleDragLeave = (e: React.DragEvent<HTMLElement>) => {
        // Возвращаем обычный курсор при уходе с файлом из компонента
        e.currentTarget.style.cursor = 'default'; // Меняем курсор на 'default'
      };
    const handleDrop = (e: React.DragEvent<HTMLElement>) => {
        e.preventDefault(); // Предотвращаем стандартное поведение при сбросе файла
        e.currentTarget.style.cursor = 'default'; 
    
        const file = e.dataTransfer?.files; // Получаем первый файл, перетащенный на компонент
    
        if (file) {
          // Обработка файла, например, загрузка, чтение и т.д.
          AddImages(Array.from(file));
        }
      };
    const onFileInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if(event.target.files !== null)
        {
            const files = Array.from(event.target.files);
            AddImages(files);
        }
    };
    return <div className="image-selector" onDragOver={handleDragOver} onDrop={handleDrop} onDragLeave={handleDragLeave}>
        <div id='image-selector-container' className='container'>
            {/*Пример item
            <div className="item">
                <img className='item-icon'/>
                <p className='item-name'>название картинки</p>
            </div>
            */}
        </div>
        <div className='image-uploader-button'>
            <label htmlFor="image-selector-file-upload" className="image-label-input">
                Выбрать файл
            </label>
            <input id='image-selector-file-upload' className='image-input' type="file" onChange={onFileInputChange} multiple/>
        </div>
    </div>
}

export default ImageSelector