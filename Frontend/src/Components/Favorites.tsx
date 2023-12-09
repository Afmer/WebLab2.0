import axios from "axios";
import { Component } from "react";
import { Link } from "react-router-dom";
import ShortShow from "../Interfaces/ShortShow";
import { AppStore } from "../AppStore";
import { inject, observer } from "mobx-react";
import '../CSS/RecycleBinButton.css'

interface Props {
    appStore?: AppStore
}
class Favorites extends Component<Props> {
    protected static _shows : ShortShow[] | null = null;
    constructor(props: any) {
        super(props);
    }
    handleFavoriteClick = (show: ShortShow) => {
        const element = document.getElementById('favorite-' + show.id);
        if(element)
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
    render() {
        return (
            <div className='shows'>
                <table>
                    {this.props.appStore?.favoriteShows?.map((item, index) => (
                        <tr><td>
                                {this.props.appStore?.authInfo?.IsAuthorize ?(
                                <div className='recycle-bin-button'>
                                    <button id={'favorite-' + item.id} 
                                            onClick={(event) => {this.handleFavoriteClick({id: item.id, name: item.name})}}>
                                    </button>
                                </div>
                            ): null}
                            <div className='background'><Link to={"/Show/" + item.id}>{item.name}</Link></div>
                        </td></tr>
                    ))}
                </table>
            </div>
            );
    }
}

export default inject('appStore')(observer(Favorites));