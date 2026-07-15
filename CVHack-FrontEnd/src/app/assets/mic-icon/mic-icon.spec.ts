import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MicIcon } from './mic-icon';

describe('MicIcon', () => {
  let component: MicIcon;
  let fixture: ComponentFixture<MicIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MicIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(MicIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
