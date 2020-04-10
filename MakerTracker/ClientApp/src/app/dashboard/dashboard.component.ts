import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { BackendService } from '../backend/backend.service';
import { MatDialog } from '@angular/material/dialog';
import { AddInventoryComponent } from './dialogs/add-inventory/add-inventory.component';
import { AddInventoryDto } from 'autogen/AddInventoryDto';
import { InventoryProductSummaryDto } from 'autogen/InventoryProductSummaryDto';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent implements OnInit {
  /** Based on the screen size, switch from standard to one column per row */
  cards = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map(({ matches }) => {
      if (matches) {
        return [
          { title: 'Card 1', cols: 1, rows: 1 },
          { title: 'Card 2', cols: 1, rows: 1 },
          { title: 'Card 3', cols: 1, rows: 1 },
          { title: 'Card 4', cols: 1, rows: 1 }
        ];
      }

      return [
        { title: 'Card 1', cols: 2, rows: 1 },
        { title: 'Card 2', cols: 1, rows: 1 },
        { title: 'Card 3', cols: 1, rows: 2 },
        { title: 'Card 4', cols: 1, rows: 1 }
      ];
    })
  );
  inventorySummary: InventoryProductSummaryDto[];

  constructor(private breakpointObserver: BreakpointObserver, private backend: BackendService, public dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.refreshDashBoard();
  }

  private refreshDashBoard() {
    this.backend.getInventorySummary().subscribe(res => this.inventorySummary = res);
  }

  openAddInventoryDialog(): void {
    const dialogRef = this.dialog.open(AddInventoryComponent, {
      data: <AddInventoryDto>{}
    });

    dialogRef.afterClosed().subscribe(result => {
      this.refreshDashBoard();
    });
  }

}
