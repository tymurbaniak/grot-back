import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FileUpload } from 'primeng/fileupload';
import { ParameterValue } from '../../models/parameter-value';
import { ComService } from '../../services/com.service';
import { ProcessService } from '../../services/process.service';

@Component({
  selector: 'app-image-editor',
  templateUrl: './image-editor.component.html',
  styleUrls: ['./image-editor.component.scss']
})
export class ImageEditorComponent implements AfterViewInit, OnInit {

  private context2d: CanvasRenderingContext2D | undefined;
  private penDown = false;
  private inputParameters: ParameterValue[] = [];

  public colors = ['#f00', '#ff0', '#0f0', '#0ff', '#00f', '#f0f', '#000', '#fff'];
  public sizes = [
    { name: '3', value: 3 },
    { name: '5', value: 5 },
    { name: '10', value: 10 },
    { name: '15', value: 15 }
  ];
  public selectedColor = '#000';
  public selectedSize = '3';

  set context(ctx: CanvasRenderingContext2D) {
    this.context2d = ctx;
  }

  get context(): CanvasRenderingContext2D {
    return this.context2d as CanvasRenderingContext2D;
  }

  @ViewChild('canv', { static: true }) private canvas: ElementRef<HTMLCanvasElement> | undefined;
  @ViewChild('fileUpload', { static: true }) private fileUpload: FileUpload | undefined;

  constructor(
    private processService: ProcessService,
    private comService: ComService
  ) { }

  ngOnInit(): void {
    this.comService.parameters$.subscribe(parameters => {
      this.inputParameters = parameters;
    });
  }

  public ngAfterViewInit(): void {
    if (this.canvas) {
      this.context = this.canvas.nativeElement.getContext("2d") as CanvasRenderingContext2D;
    }

    this.onResize();
  }

  public onResize(): void {
    if (this.canvas) {
      this.canvas.nativeElement.height = this.canvas.nativeElement.clientHeight;
      this.canvas.nativeElement.width = this.canvas.nativeElement.clientWidth;
    }
  }

  public startDrawing($event: MouseEvent): void {
    const position = this.getMousePosition($event);
    this.penDown = true;

    this.context.beginPath();
    this.context.moveTo(position.x, position.y);
    this.context.stroke();
  }

  public drawing($event: MouseEvent): void {
    if (this.penDown) {
      const position = this.getMousePosition($event);

      this.context.lineTo(position.x, position.y);
      this.context.stroke();
    }
  }

  public stopDrawing($event: MouseEvent): void {
    const position = this.getMousePosition($event);
    this.penDown = false;
    this.context.lineTo(position.x, position.y);
    this.context.stroke();
  }

  private getMousePosition($event: MouseEvent): { x: number, y: number } {
    let cords = { x: 0, y: 0 };

    if (this.canvas) {
      const rect = this.canvas.nativeElement.getBoundingClientRect();
      cords.x = $event.clientX - rect.left;
      cords.y = $event.clientY - rect.top;
    }

    return cords;
  }

  public setColor($color: string): void {
    this.selectedColor = $color;
    this.context.strokeStyle = $color;
  }

  public sizeChanged($size: any): void {
    this.context.lineWidth = $size.value.value;
  }

  public setCanvasImage($event: any): void {
    const files = $event.files;
    const img = new Image();
    img.onload = () => {
      if (this.canvas) {
        const width = this.canvas.nativeElement.clientWidth;
        const height = this.canvas.nativeElement.clientHeight;
        this.context.drawImage(img, 0, 0, width, height);
      }
    };
    img.src = URL.createObjectURL(files[0]);

    if (this.fileUpload) {
      this.fileUpload.clear();
    }
  }

  public onProcessClicked($event: any): void {
    if(this.canvas){
      const canvasDataUrl = this.canvas.nativeElement.toDataURL();

      this.processService.process(this.inputParameters, canvasDataUrl).subscribe(result => {
        console.error(result);
      });
    }  
  }

}
