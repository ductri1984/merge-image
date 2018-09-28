import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoadingController } from 'ionic-angular';
import { Clipboard } from '@ionic-native/clipboard';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {
  urlget:string = 'http://svn.3ps.vn:15002/api/Data/GetData';
  lst:any;

  constructor(public navCtrl: NavController, private http: HttpClient, private clipboard: Clipboard, public loadingCtrl: LoadingController) {

  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad HomePage');
    
    this.http.post(this.urlget, JSON.stringify({ }), 
      { headers: new HttpHeaders({ 'Content-Type':'application/json' }) }).subscribe(res => {
        this.lst = res;
      }, (err) => {
      
        console.log(err);
      });
  }

  clickCopy(link){
    console.log(link);
    this.clipboard.copy(link);
    const loader = this.loadingCtrl.create({
      content: "Copy to clipboard",
      duration: 3000
    });
    loader.present();
  }
}
