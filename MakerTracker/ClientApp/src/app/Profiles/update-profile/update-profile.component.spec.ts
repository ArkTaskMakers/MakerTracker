import {
  async,
  ComponentFixture,
  TestBed,
  inject,
} from "@angular/core/testing";
import { ReactiveFormsModule } from "@angular/forms";
import { NoopAnimationsModule } from "@angular/platform-browser/animations";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatInputModule } from "@angular/material/input";
import { MatRadioModule } from "@angular/material/radio";
import { MatSelectModule } from "@angular/material/select";

import { UpdateProfileComponent } from "./update-profile.component";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatButtonToggleModule } from "@angular/material/button-toggle";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { HttpClientModule } from "@angular/common/http";
import {
  HttpClientTestingModule,
  HttpTestingController,
} from "@angular/common/http/testing";
import { ProfileDto } from "autogen/ProfileDto";
import { BackendService } from "src/app/services/backend/backend.service";
import { of } from "rxjs";

class MockSnackBar {
  open = jasmine.createSpy();
}

class MockBackendService {
  payload: any = null;
  saveProfile = jasmine.createSpy().and.callFake(profile => {
    this.payload = profile;
    return of(profile);
  });
  getProfile = jasmine.createSpy().and.returnValue(of(new ProfileDto({
    firstName: 'John',
    lastName: 'Doe',
    email: 'test@example.com',
    city: 'test',
    state: 'AR',
    zipCode: '72120',
    isRequestor: true
  })));
}

describe("UpdateProfileComponent", () => {
  let component: UpdateProfileComponent;
  let fixture: ComponentFixture<UpdateProfileComponent>;
  let backend: HttpTestingController;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UpdateProfileComponent],
      imports: [
        NoopAnimationsModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatCardModule,
        MatInputModule,
        MatRadioModule,
        MatSelectModule,
        MatSlideToggleModule,
        MatButtonToggleModule,
        HttpClientModule,
        HttpClientTestingModule,
      ],
      providers: [
        { provide: "BASE_URL", useValue: "", deps: [] },
        { provide: MatSnackBar, useClass: MockSnackBar },
        { provide: BackendService, useClass: MockBackendService },
      ],
    }).compileComponents();
  }));

  afterEach(inject(
    [HttpTestingController],
    (backend: HttpTestingController) => {
      backend.verify();
    }
  ));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    backend = TestBed.get(HttpTestingController);
  });

  it("should edit profile", () => {
    const backendSvc: MockBackendService = TestBed.get(BackendService);
    backend = TestBed.get(HttpTestingController);
    expect(component).toBeTruthy();
    component.onSubmit();
    expect(backendSvc.saveProfile).toHaveBeenCalled();
    expect(backendSvc.payload.firstName).toBe('John');
  });

  it("should edit profile", () => {
    const backendSvc: MockBackendService = TestBed.get(BackendService);
    const snack: MockSnackBar = TestBed.get(MatSnackBar);
    backend = TestBed.get(HttpTestingController);
    expect(component).toBeTruthy();
    component.patchValues(new ProfileDto());
    component.onSubmit();
    expect(backendSvc.saveProfile).not.toHaveBeenCalled();
    expect(snack.open).toHaveBeenCalledWith('Please Validate your Profile', null, {
      duration: 2000,
    });
  });
});
