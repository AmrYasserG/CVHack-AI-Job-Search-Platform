import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MailIcon } from './mail-icon';

describe('MailIcon', () => {
  let component: MailIcon;
  let fixture: ComponentFixture<MailIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MailIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(MailIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
