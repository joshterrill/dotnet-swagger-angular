import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SampleDataClient, WeatherForecast } from '../app.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(private _appService: SampleDataClient) {
    this._appService.weatherForecasts().subscribe(result => this.forecasts = result);
    // http.get<WeatherForecast[]>(baseUrl + 'api/SampleData/WeatherForecasts').subscribe(result => {
    //   this.forecasts = result;
    // }, error => console.error(error));
  }
}
