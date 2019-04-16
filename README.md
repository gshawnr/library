# LittleLibrary


### Summary
A platform that allows established and budding authors to publish and retail their work online.  The site will allow authors to publish content of various formats from articles and short stories to full length novels. 

[OneStopShop](https://docs.google.com/document/d/1qrxTcHthvI1PUPQSwg_e3Bfq_Ciz7zvudymixWPFFB8/edit): Planning Documents

* Admin
  * Alter Users/Roles
  * Test Admin:
      * Email: gshawnr@gmail.com
      * Password: P@ssw0rd!
* Authors
  * Upload books
  * Remove own books
  * Test Author:
     * Email: CharlesDickens@email.com
     * Password: P@ssw0rd!
* Users
  * Add books to cart
  * Purchase books
  * Test User:
     * Email: philipw@professor.gov
     * Password: P@ssw0rd!

[Little Library](https://littlelibrary20190125030031.azurewebsites.net/): Hosted Application





We have added an API for the Books portion of our application. Included in the javascript are the relevant fields for each book.

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



