import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';


import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { Ng2TableModule } from 'ng2-table/ng2-table';
import { AlertModule } from 'ng2-bootstrap/ng2-bootstrap';
import { TabsModule } from 'ng2-bootstrap/ng2-bootstrap';
import { DatepickerModule } from 'ng2-bootstrap/ng2-bootstrap';
import { TooltipModule } from 'ng2-bootstrap/ng2-bootstrap';


import { AppComponent }  from './app.component';
import { WelcomeComponent } from './home/welcome.component';

/* Feature Modules */
import { ProductModule } from './products/product.module';
import { ISummary } from './models/summary';
import { CLOService } from './services/closervice';

@NgModule({
  imports: [
      BrowserModule, AlertModule,
    HttpModule,
    RouterModule.forRoot([
      { path: 'welcome', component: WelcomeComponent },
      { path: '', redirectTo: 'welcome', pathMatch: 'full' },
      { path: '**', redirectTo: 'welcome', pathMatch: 'full' }
    ]),
    AlertModule.forRoot(),
    TabsModule.forRoot(),
    DatepickerModule.forRoot(),
    TooltipModule.forRoot(),
      FormsModule,
      Ng2TableModule,
    ProductModule
    ],
  declarations: [
    AppComponent,
    WelcomeComponent
  ],
  providers: [
      CLOService
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
