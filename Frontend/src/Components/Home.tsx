import React, { useState, useEffect, Component } from 'react';
interface PartViewData {
    html: string;
  }
class Home extends Component <{}, PartViewData> {
    protected static html : PartViewData | null = null;
    constructor(props: any) {
        super(props);
        if(Home.html === null){
            this.state = {
                html: ""
            }
            fetch('/api/Home')  // Путь начинается с '/'
            .then(response => {
                if (!response.ok) {
                throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then((data : PartViewData) => {
                this.setState(data);
                Home.html = data
            })
            .catch(error => {
                console.error('Error:', error);
            });
        }
        else{
            this.state = Home.html
        }
    }
    render(){
        return (
            <div>
                <div dangerouslySetInnerHTML={{ __html: this.state.html }} />
            </div>
        )
    }
}

export default Home