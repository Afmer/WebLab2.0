import axios from "axios";
import { Component } from "react";
import { Link } from "react-router-dom";
import ShortShow from "../Interfaces/ShortShow";
import { AppStore } from "../AppStore";
import { inject, observer } from "mobx-react";

interface Props {
    appStore?: AppStore
}
class Favorites extends Component<Props> {
    protected static _shows : ShortShow[] | null = null;
    constructor(props: any) {
        super(props);
    }
    render() {
        return (
            <div className='shows'>
                <table>
                    {this.props.appStore?.favoriteShows?.map((item, index) => (
                        <tr><td>
                            <div className='background'><Link to={"/Show/" + item.id}>{item.name}</Link></div>
                        </td></tr>
                    ))}
                </table>
            </div>
            );
    }
}

export default inject('appStore')(observer(Favorites));