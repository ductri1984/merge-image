import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';

import { Storage } from '@ionic/storage';
import { AlertController } from 'ionic-angular';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@IonicPage()
@Component({
  selector: 'page-settings',
  templateUrl: 'settings.html',
})
export class SettingsPage {
  action:string;
  filename:string;
  token:string;
  urljoin:string = 'http://svn.3ps.vn:15002/api/Data/SendJoin';
  urlleave:string = 'http://svn.3ps.vn:15002/api/Data/SendLeave';

  constructor(public navCtrl: NavController, public navParams: NavParams,
    private storage: Storage, private http: HttpClient,
    public alertCtrl: AlertController) {
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad SettingsPage');

    this.action = 'action';
    this.storage.get('token').then(val => {
      this.token = val + '';
    });
    this.storage.get('filename').then(val => {
      this.filename = val;
    });
  }

  saveData(){
    if(this.token != null && this.token != ''){
      this.leave().then((res)=> {        
        if(this.filename != null && this.filename != ''){
          this.join().then(()=>{
            console.log('join');
            
          }, (err) => {
            console.log(err);
          });          
        }
        this.storage.set('filename', this.filename);
      },(err) =>{
        console.log(err);
      });
    }
  }

  leave(){
    //res.header("Access-Control-Allow-Origin", "*");
    return new Promise((resolve, reject) => {
      this.storage.get('filename').then((val) => {
        if(val != null && val != '' && val != this.filename){
          this.action = 'leaving ...';
          const headers =  new HttpHeaders();
          headers.append('Content-Type','application/json');
          headers.append('Access-Control-Allow-Origin','*');
          this.http.post(this.urlleave, JSON.stringify({
            Token: this.token,
            Topic: val
          }), { headers: headers })
          .toPromise().then((res) => {
            this.action = 'leaved';
            setTimeout(()=>{
              //this.action = '';
            }, 2000);
            resolve(res);
          },(err) => {
            //this.action = err;
            reject(err);
          });
        }
        else{
          reject();
        }
      },(err) => {
        reject(err);
      });
    });
  }

  join(){
    return new Promise((resolve, reject) => {
      this.storage.get('filename').then(val => {
        if(this.filename != null && this.filename != ''){        
          this.action = 'joining ...';
          const headers =  new HttpHeaders();
          headers.append('Content-Type','application/json');
          headers.append('Access-Control-Allow-Origin','*');
          this.http.post(this.urljoin, JSON.stringify({
            Token: this.token,
            Topic: this.filename
          }), { headers: headers })
          //}), { headers: new HttpHeaders({'Content-Type': 'application/json'}) })
          .toPromise().then(res => {   
            this.action = 'joined';
            setTimeout(()=>{
              //this.action = '';
            }, 2000);
            resolve(res);
          }, (err) => {       
            //this.action = err;      
            reject(err);
          });
        }
        else{
          reject();
        }        
      });
    });
  }
}
