import axios from 'axios';
import React, { Component, MouseEventHandler } from 'react';
import '../CSS/Shows.css'
import '../CSS/FavoritePinButton.css'
import { Routes, Route, Link } from 'react-router-dom';
import { inject, observer } from 'mobx-react';
import { AppStore } from '../AppStore';
import '../CSS/RecycleBinButton.css'
import ShortShow from '../Interfaces/ShortShow';
interface States {
    Shows: ShortShow[];
}
interface Props {
    appStore?: AppStore
}
class Shows extends Component<Props, States> {
    protected static _shows : ShortShow[] | null = null;
    constructor(props: any) {
        super(props);
        if(Shows._shows === null)
        {
            const showResponse = axios.get('api/Shows')
                .then(response => {
                    Shows._shows = response.data as ShortShow[]
                    this.setState({Shows: response.data as ShortShow[]});
                })
                .catch(error => {
                    console.error('Произошла ошибка:', error);
                });
        }
        this.state ={
            Shows: Shows._shows !== null ? (Shows._shows) : ([]),
        }
    }
    handleFavoriteClick = (show: ShortShow) => {
        const element = document.getElementById('favorite-' + show.id);
        if(element)
        {
            if(element.className === 'unpinned')
            {
                const pinResponse = axios.get('api/FavoriteShow/Add?showId=' + show.id)
                .then(response =>{
                    if (response.status === 200)
                    {
                        this.props.appStore?.addFavoriteShow(show);
                    }
                })
                .catch(error => {
                    console.error('Произошла ошибка:', error);
                });
            }
            else if(element.className === 'pinned')
            {
                const pinResponse = axios.get('api/FavoriteShow/Delete?showId=' + show.id)
                .then(response =>{
                    if (response.status === 200)
                    {
                        this.props.appStore?.removeFavoriteShow(show)
                    }
                })
                .catch(error => {
                    console.error('Произошла ошибка:', error);
                });
            }
        }
    }
    handleDeleteClick = (show: ShortShow) => {
        const form = new FormData();
        form.append('showId', show.id)
        axios.post('api/Shows/Delete', form)
            .then(response => {
                if(response.status === 200)
                {
                    this.props.appStore?.removeFavoriteShow(show);
                    const newShowList = Shows._shows?.filter(x => x.id != show.id)
                    if(newShowList !== undefined)
                    {
                        Shows._shows = newShowList;
                        this.setState({Shows: newShowList})
                    }
                    else
                    {
                        Shows._shows = []
                        this.setState({Shows: []})
                    }
                }
            })
            .catch((error) => {
                console.error('Ошибка:', error.config);
            });
    }
    render() {
        return (
        <div className='shows'>
            <table>
                {this.state.Shows.map((item, index) => (
                    <tr><td>
                        <div className='background'>
                            {this.props.appStore?.authInfo?.IsAuthorize ?(
                                <>
                                <div className='favorite-pin-button'>
                                    <button id={'favorite-' + item.id} 
                                            className={this.props.appStore.favoriteShows?.find((obj) => obj.id === item.id) ?('pinned'):('unpinned')} 
                                            onClick={(event) => {this.handleFavoriteClick({id: item.id, name: item.name})}}>
                                    </button>
                                </div>
                                {this.props.appStore.authInfo.IsAdmin &&(<div className='recycle-bin-button'>
                                    <button id={'favorite-' + item.id} 
                                            className={this.props.appStore.favoriteShows?.find((obj) => obj.id === item.id) ?('pinned'):('unpinned')} 
                                            onClick={(event) => {this.handleDeleteClick({id: item.id, name: item.name})}}>
                                    </button>
                                </div>)}
                                </>
                            ): null}
                            <Link to={"/Show/" + item.id}>{item.name}</Link>
                        </div>
                    </td></tr>
                ))}
            </table>
        </div>
        );
    }
}

export default inject('appStore')(observer(Shows))