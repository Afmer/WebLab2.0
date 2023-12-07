import axios from "axios";
import { Component } from "react";
import { Link } from "react-router-dom";

interface ShortShow {
    id : string,
    name : string
}
interface States {
    Shows: ShortShow[]
}
class Favorites extends Component<{}, States> {
    protected static _shows : ShortShow[] | null = null;
    constructor(props: any) {
        super(props);
        if(Favorites._shows === null)
        {
            this.state = {
                Shows: []
            };
            const response = axios.get('api/FavoriteShow')
                .then(response => {
                    Favorites._shows = response.data as ShortShow[]
                    this.setState({Shows: response.data as ShortShow[]});
                })
                .catch(error => {
                    console.error('Произошла ошибка:', error);
                });
        }
        else
        {
            this.state = {
                Shows: Favorites._shows
            }
        }
    }
    render() {
        return (
            <div className='shows'>
                <table>
                    {this.state.Shows.map((item, index) => (
                        <tr><td>
                            <div className='background'><Link to={"/Show/" + item.id}>{item.name}</Link></div>
                        </td></tr>
                    ))}
                </table>
            </div>
            );
    }
}

export default  Favorites;