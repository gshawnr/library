# LittleLibrary


### Summary
A project to create a platform that allows established and budding authors to publish and retail their work online.  The site would allow authors to publish content of various formats from articles and short stories to full length novels. 

[Little Library](https://littlelibrary-gsr.azurewebsites.net/): Hosted Application

API for the Books portion of our application. Included in the javascript are the relevant fields for each book.

Javascript Script:

```
var request = new XMLHttpRequest();

request.open('GET', 'https://littlelibrary20190125030031.azurewebsites.net/api/books/', true);

request.onload = function (){

    var data = JSON.parse(this.response);

    if(request.status >= 200 && request.status < 400){
        data.forEach(book =>{
            console.log(book.title);
            console.log(book.summary);
            console.log(book.genre);
            console.log(book.price);
        });
    } else {
        console.log('error');
    }
}

request.send();

```



