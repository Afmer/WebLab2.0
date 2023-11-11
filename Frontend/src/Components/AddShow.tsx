import React, { ChangeEvent, useState } from 'react';

function AddShow() {
    const [DictFile, setDictFile] = useState<Record<string, File>>();
    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files && event.target.files[0];
        const name = event.target.name;
        //setDictFile({...DictFile, name: file})
    }
    return (
        <input
        name='0'
        type="file"
        accept="image/*"
        onChange={handleFileChange}
      />
    )
}

export default AddShow