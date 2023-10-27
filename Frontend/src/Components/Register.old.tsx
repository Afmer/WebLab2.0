import axios from 'axios';
import React, { ChangeEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';

function Register() {
    const [formData, setFormData] = useState(() => {
      return {
        Login: '',
        Password: '',
        Email: '',
      }
    });
    const navigate = useNavigate();
    const changeRegisterInfo = (event: ChangeEvent<HTMLInputElement>) => {
      event.persist();
      setFormData(prev => {
        return {
            ...prev,
            [event.target.name]: event.target.value,
        }
      });
    };
    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        try {
            const response = await axios.post('/api/Identity/Register', formData);
      
            if (response.status === 200) {
              console.log('Регистрация успешно завершена!');
              navigate('/');
            }
          } catch (error) {
            console.error('Ошибка:', error);
          }
    }
    return (
        <div>
          <form onSubmit={handleSubmit}>
            <input id="Login" name="Login" type="username" value={formData.Login} onChange={changeRegisterInfo}/>
            <input id="Password" name="Password" type="password" value={formData.Password} onChange={changeRegisterInfo}/>
            <input id="Email" name="Email" type="email" value={formData.Email} onChange={changeRegisterInfo}/>
            <input type="submit"/>
          </form>
        </div>
    )
}

export default Register