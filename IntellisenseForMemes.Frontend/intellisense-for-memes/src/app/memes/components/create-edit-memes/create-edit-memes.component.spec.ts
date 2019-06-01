import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditMemesComponent } from './create-edit-memes.component';

describe('CreateEditMemesComponent', () => {
  let component: CreateEditMemesComponent;
  let fixture: ComponentFixture<CreateEditMemesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditMemesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditMemesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
