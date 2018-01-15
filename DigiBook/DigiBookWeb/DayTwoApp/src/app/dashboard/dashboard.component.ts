import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { UIChart } from 'primeng/primeng';
// tslint:disable-next-line:import-blacklist
import { Observable } from 'rxjs';

const DEFAULT_COLORS = ['#3366CC', '#DC3912', '#FF9900', '#109618', '#990099',
  '#3B3EAC', '#0099C6', '#DD4477', '#66AA00', '#B82E2E',
  '#316395', '#994499', '#22AA99', '#AAAA11', '#6633CC',
  '#E67300', '#8B0707', '#329262', '#5574A6', '#3B3EAC'];

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})


export class DashboardComponent implements OnInit, AfterViewInit {

  @ViewChild('mixedChart') mixedChart: UIChart;

  hoursByProject = [
    { id: 1, name: 'Payroll App', hoursSpent: 8 },
    { id: 2, name: 'Agile Times App', hoursSpent: 16 },
    { id: 3, name: 'Point of Sale App', hoursSpent: 24 }
  ];

  pieLabels = this.hoursByProject.map((proj) => proj.name);
  pieData = this.hoursByProject.map((proj) => proj.hoursSpent);
  pieColors = this.configureDefaultColours(this.pieData);

  private configureDefaultColours(data: number[]): string[] {
    let customColours = [];
    if (data.length) {

      customColours = data.map((element, idx) => {
        return DEFAULT_COLORS[idx % DEFAULT_COLORS.length];
      });
    }

    return customColours;
  }

  // tslint:disable-next-line:member-ordering
  hoursByProjectChartData = {
    labels: ['Payroll App', 'Agile Times App', 'Point ofSales App'],
    datasets: [
      {
        data: this.pieData,
        backgroundColor: this.pieColors
      }
    ]
  };

  // tslint:disable-next-line:member-ordering
  hoursByTeamChartData = {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
    datasets: [
      {
        label: 'Dev Team',
        backgroundColor: DEFAULT_COLORS[0],
        data: [44, 63, 57, 90, 77, 73]

      },
      {
        label: 'Ops Team',
        backgroundColor: DEFAULT_COLORS[1],
        data: [65, 59, 80, 57, 67, 70]

      }
    ]
  };

  // tslint:disable-next-line:member-ordering
  hoursByTeamChartDataMixed = {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
    datasets: [
      {
        label: 'Dev Team',
        type: 'bar',
        backgroundColor: DEFAULT_COLORS[0],
        data: [44, 63, 57, 90, 77, 73]

      },
      {
        label: 'Ops Team',
        type: 'line',
        backgroundColor: DEFAULT_COLORS[1],
        data: [65, 59, 80, 57, 67, 70]

      }
    ]
  };
  constructor() {

  }

  onDataSelect(event) {
    // tslint:disable-next-line:prefer-const
    let dataSetIndex = event.element._datasetIndex;
    // tslint:disable-next-line:prefer-const
    let dataItemIndex = event.element._index;

    // tslint:disable-next-line:prefer-const
    let labelClicked = this.hoursByTeamChartDataMixed.datasets[dataSetIndex].label;
    // tslint:disable-next-line:prefer-const
    let valueClicked = this.hoursByTeamChartDataMixed.datasets[dataItemIndex].data[dataItemIndex];

    alert(`${labelClicked}-${valueClicked}`);
  }

  ngOnInit() {

  }

  ngAfterViewInit(): void {
    Observable.interval(3000).timeInterval().subscribe(() => {
      // tslint:disable-next-line:prefer-const
      let hoursByTeam = this.hoursByTeamChartDataMixed.datasets;
      // tslint:disable-next-line:prefer-const
      let randomized = hoursByTeam.map((dataset) => {
        dataset.data = dataset.data.map((hours) => hours * (Math.random() * 2));
      });
      this.mixedChart.refresh();
    });
  }
}
