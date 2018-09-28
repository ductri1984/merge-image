import { Component } from '@angular/core';
import { Platform } from 'ionic-angular';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';

import { TabsPage } from '../pages/tabs/tabs';

import { FCM } from '@ionic-native/fcm';
import { Storage } from '@ionic/storage';
import { LoadingController } from 'ionic-angular';
import { AlertController } from 'ionic-angular';

@Component({
  templateUrl: 'app.html'
})
export class MyApp {
  rootPage:any = TabsPage;

  constructor(platform: Platform, statusBar: StatusBar, splashScreen: SplashScreen,
    private fcm: FCM, private storage: Storage, 
    public loadingCtrl: LoadingController, public alertCtrl: AlertController) {
    platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.
      // Here you can do any higher level native things you might need.
      statusBar.styleDefault();
      splashScreen.hide();
      this.setup();
    });
  }

  setup(){
    this.fcm.subscribeToTopic('all');
    this.fcm.getToken().then(token => {
          console.log(token);
          if(token != null && token != ''){
            this.storage.set('token', token);
          }
    });
    this.fcm.onNotification().subscribe(data => {
          if (data.wasTapped) {
            console.log("Received in background");
            let alert = this.alertCtrl.create({
              title: '1',
              subTitle: '1',
              buttons: ['OK']
            });
            alert.present();
          } else {
            console.log("Received in foreground");
            let alert = this.alertCtrl.create({
              subTitle: 'Quý khách có một tin nhắn mới <br> Vui lòng vào mục thông báo để xem',
              buttons: ['OK']
            });
            alert.present();
          };
    });
    this.fcm.onTokenRefresh().subscribe(token => {
      console.log(token);
      if(token != null && token != ''){
        this.storage.set('token', token);
      }
    });
  }
}
