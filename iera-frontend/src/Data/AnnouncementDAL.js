import APIAdress from "./APIAdress"

function GetAllAnnouncements(){
    fetch(APIAdress).then(response => response.json()).then(data => console.log(data))
}

export default GetAllAnnouncements