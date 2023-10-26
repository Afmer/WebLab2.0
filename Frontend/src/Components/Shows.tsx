import axios from 'axios';
import React, { Component } from 'react';
import '../CSS/Shows.css'
import { Routes, Route } from 'react-router-dom';
interface ShortShow {
    id : string,
    name : string
}
interface States {
    Shows: ShortShow[]
}
class Shows extends Component<{}, States> {
    protected static _shows : ShortShow[] | null = null;
    constructor(props: any) {
        super(props);
        if(Shows._shows === null)
        {
            this.state = {
                Shows: []
            };
            const response = axios.get('api/Shows')
                .then(response => {
                    Shows._shows = response.data as ShortShow[]
                    this.setState({Shows: response.data as ShortShow[]});
                })
                .catch(error => {
                    console.error('Произошла ошибка:', error);
                });
        }
        else
        {
            this.state = {
                Shows: Shows._shows
            }
        }
    }
    render() {
        return (
        <div className='shows'>
            <table>
                {this.state.Shows.map((item, index) => (
                    <tr><td>
                        <div className='background'><a href="">{item.name}</a></div>
                    </td></tr>
                ))}
            </table>
        </div>
        );
    }
}

export default Shows