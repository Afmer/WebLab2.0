import axios from "axios";
import IdentityBase from "../../Interfaces/IdentityBase";
import { AppStore } from "../../AppStore";

const InitIdentity = async (appStore: AppStore) => {
    await axios.get('/api/Identity/WhoIAm')
        .then(function (response) {
            if(response.statusText !== 'OK')
                appStore?.updateAuth({IsAuthorize: false, IsAdmin: false, Username: ""})
            else
            {
                const data : IdentityBase = response.data;
                appStore?.updateAuth({IsAuthorize: true, IsAdmin: false, Username: data.login})
            }
        })
        .catch(error => {
            appStore?.updateAuth({IsAuthorize: false, IsAdmin: false, Username: ""})
            console.error('Error:', error);
        })
}

export default InitIdentity