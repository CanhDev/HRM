import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { EmployeeDocumentService } from '../../document.service';
import { forkJoin, map } from 'rxjs';
import { 
  EmployeeDocument, 
  EmployeeDocumentFolder, 
  EmployeeDocumentDTO,
  DocumentTag 
} from '../../entity';

@Component({
  selector: 'app-employee-document',
  templateUrl: './document.component.html',
  styleUrls: ['./document.component.css']
})
export class EmployeeDocumentComponent implements OnInit {
  // Core data
  employeeId: number = 0;
  folders: EmployeeDocumentFolder[] = [];
  selectedFolder: EmployeeDocumentFolder | null = null;
  
  // Document management
  documents: EmployeeDocument[] = [];
  selectedDocuments: number[] = [];
  searchTerm: string = '';
  selectedTags: string[] = [];
  isMovingDocuments: boolean = false;
  targetFolderId: number | null = null;

  isOpeningUploadModal: boolean = false;
isOpeningFolderModal: boolean = false;
isOpeningDocumentModal: boolean = false;
  
  // File management
  selectedFiles: File[] = [];
  allowedFileTypes: string[] = ['pdf', 'doc', 'docx', 'xls', 'xlsx', 'jpg', 'jpeg', 'png', 'txt'];

  // Forms
  documentForm!: FormGroup;
  folderForm!: FormGroup;
  uploadForm!: FormGroup;
  
  // UI States
  loading: boolean = false;
  showUploadModal: boolean = false;
  showFolderModal: boolean = false;
  showDocumentModal: boolean = false;
  editMode: boolean = false;
  
  // Tags management
  allTags: DocumentTag[] = [];
  
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private documentService: EmployeeDocumentService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.employeeId = +params['id'] || 0;
      this.loadFolders();
    });

    // Initialize forms
    this.initForms();
    
    // Load tags for filtering
    this.loadTags();
  }

  initForms(): void {
    this.folderForm = this.fb.group({
      id: [0],
      folderName: ['', Validators.required],
      description: [''],
      employeeId: [this.employeeId]
    });
    
    this.documentForm = this.fb.group({
      id: [0],
      documentName: ['', Validators.required],
      notes: [''],
      tags: [''],
      expiryDate: [null],
      folderId: [null],
      employeeId: [this.employeeId]
    });
    
    this.uploadForm = this.fb.group({
      files: [null]
    });
  }

  // ==================
  // FOLDER OPERATIONS
  // ==================
  
  loadFolders(): void {
    console.log(this.employeeId);
  this.loading = true;
  this.documentService.getFoldersByEmployeeId(this.employeeId).subscribe({
    next: (res) => {
      if (res.success) {
        this.folders = res.data;
        
        // Get document counts for each folder
        const countPromises = this.folders.map(folder => 
          this.documentService.getDocumentsByFolderId(folder.id).pipe(
            map(res => {
              if (res.success) {
                // Update folder with document count
                folder.documents = res.data;
                return folder;
              }
              return folder;
            })
          )
        );
        forkJoin(countPromises).subscribe({
          next: (updatedFolders) => {
            this.folders = updatedFolders;
            // Select first folder if available
            if (this.folders.length > 0 && !this.selectedFolder) {
              this.selectFolder(this.folders[0]);
            }
            this.loading = false;
            this.selectedFiles = [];
          },
          error: (err) => {
            console.error('Error loading folder document counts:', err);
            this.loading = false;
          }
        });
      } else {
        this.loading = false;
      }
    },
    error: (err) => {
      console.error('Error loading folders:', err);
      this.loading = false;
    }
  });
}

  selectFolder(folder: EmployeeDocumentFolder): void {
    this.selectedFolder = folder;
    this.loadDocuments(folder.id);
    this.selectedDocuments = [];
  }

  

  // ==================
  // DOCUMENT OPERATIONS
  // ==================
  
  loadDocuments(folderId: number): void {
    this.loading = true;
    this.documentService.getDocumentsByFolderId(folderId).subscribe({
      next: (res) => {
        if (res.success) {
          this.documents = res.data;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading documents:', err);
        this.loading = false;
      }
    });
  }
  
 
  
  onFileSelected(event: any): void {
    if (event.target.files) {
      this.selectedFiles = Array.from(event.target.files);
    }
  }

  
  deleteSelectedDocuments(): void {
  if (this.selectedDocuments.length === 0) return;
  
  const confirmText = this.selectedDocuments.length === 1 
    ? 'Bạn có chắc chắn muốn xóa 1 tài liệu này không?' 
    : `Bạn có chắc chắn muốn xóa ${this.selectedDocuments.length} tài liệu đã chọn không?`;
    
  if (confirm(confirmText)) {
    this.loading = true;
    
    // Delete documents one by one
    
    const deleteObservables = this.selectedDocuments.map(docId => 
      this.documentService.deleteDocument(docId)
    );
    
    forkJoin(deleteObservables).subscribe({
      next: () => {
        // Reload documents after all deletes are done
        if (this.selectedFolder) {
          this.loadDocuments(this.selectedFolder.id);
        }
        // Also reload folders to update document counts
        this.loadFolders();
        this.selectedDocuments = [];
        this.loading = false;
      },
      error: (err) => {
        console.error('Error deleting documents:', err);
        this.loading = false;
      }
    });
  }
}
deleteSelectedFolder(): void {
  if (!this.selectedFolder) return;
  
  const confirmText = "Bạn có chắc chắn xóa thư mục này không?";
    
  if (confirm(confirmText)) {
    this.loading = true;
    this.documentService.deleteFolder(this.selectedFolder.id).subscribe((res)=>{
        this.loading = false;
         this.loadFolders();
         this.selectedFolder = null;
    }, (error) => {
    console.error('Đã xảy ra lỗi:', error);
    this.loading = false;
  })
  }
}
  // ==================
  // MOVE OPERATIONS
  // ==================
  
  startMoveDocuments(): void {
    if (this.selectedDocuments.length === 0) return;
    this.isMovingDocuments = true;
    this.targetFolderId = null;
  }
  
  cancelMove(): void {
    this.isMovingDocuments = false;
    this.targetFolderId = null;
  }
  
  moveDocuments(): void {
  if (!this.targetFolderId || !this.selectedFolder || this.selectedDocuments.length === 0) return;
  
  this.loading = true;
  
  // Create observables for each document move
  
  const moveObservables = this.selectedDocuments.map(docId => {
    // Find the document
    const doc = this.documents.find(d => d.id === docId);
    if (!doc) return null;
    
    // Create DTO with updated folder ID
    const docDTO: EmployeeDocumentDTO = {
      id: doc.id,
      documentName: doc.documentName,
      folderId: this.targetFolderId!,
      employeeId: doc.employeeId,
      documentType: doc.documentType,
      tags: doc.tags,
      notes: doc.notes,
      expiryDate: doc.expiryDate,
      actionType: 'A'
    };
    
    return this.documentService.updateDocument(docDTO);
  }).filter(obs => obs !== null) as any[];
  
  forkJoin(moveObservables).subscribe({
    next: () => {
      // Reload documents after all moves are done
      if (this.selectedFolder) {
        this.loadDocuments(this.selectedFolder.id);
      }
      // Also reload folders to update document counts 
      this.loadFolders();
      this.isMovingDocuments = false;
      this.targetFolderId = null;
      this.selectedDocuments = [];
      this.loading = false;
    },
    error: (err) => {
      console.error('Error moving documents:', err);
      this.loading = false;
    }
  });
}
  // ==================
  // SELECTION & FILTERING
  // ==================
  
  toggleDocumentSelection(docId: number): void {
    const index = this.selectedDocuments.indexOf(docId);
    if (index > -1) {
      this.selectedDocuments.splice(index, 1);
    } else {
      this.selectedDocuments.push(docId);
    }
  }


  
  selectAllDocuments(): void {
    if (this.selectedDocuments.length === this.getFilteredDocuments().length) {
      // If all are selected, unselect all
      this.selectedDocuments = [];
    } else {
      // Otherwise select all filtered documents
      this.selectedDocuments = this.getFilteredDocuments().map(doc => doc.id);
    }
  }
  
getFilteredDocuments(): EmployeeDocument[] {
    if (!this.documents) return [];
    
    return this.documents.filter(doc => {
      // Filter by search term
      const matchesSearch = !this.searchTerm || 
        doc.documentName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        (doc.notes && doc.notes.toLowerCase().includes(this.searchTerm.toLowerCase()));
      
      // Filter by selected tags
      const docTags = this.formatTags(doc.tags);
      const lowerSelectedTags = this.selectedTags.map(t => t.toLowerCase());
      const matchesTags = lowerSelectedTags.length === 0 || 
        lowerSelectedTags.every(tag => docTags.includes(tag));
      
      return matchesSearch && matchesTags;
    });
  }
  
toggleTagSelection(tagName: string): void {
    const lowerTagName = tagName.toLowerCase();
    const index = this.selectedTags.findIndex(t => t.toLowerCase() === lowerTagName);
    if (index > -1) {
      this.selectedTags.splice(index, 1);
    } else {
      this.selectedTags.push(tagName);
    }
  }

  // ==================
  // TAG OPERATIONS
  // ==================
  getAvailableTags(): string {
    return this.allTags.map(t => t.name).join(', ');
  }
  loadTags(): void {
    this.documentService.getAllTags().subscribe({
      next: (res) => {
        if (res.success) {
          this.allTags = res.data;
        }
      },
      error: (err) => console.error('Error loading tags:', err)
    });
  }
  
formatTags(tagsString?: string): string[] {
    if (!tagsString) return [];
    return tagsString.split(',').map(tag => tag.trim().toLowerCase()).filter(tag => tag);
  }
  
  
  getTagColor(tagName: string): string {
    const tag = this.allTags.find(t => t.name.toLowerCase() === tagName.toLowerCase());
    return tag ? `badge-${tag.color}` : 'badge-secondary';
  }
  
  // ==================
  // UTILITY METHODS
  // ==================
  
  getFileIcon(fileType?: string): string {
    if (!fileType) return 'fa-file';
    
    switch(fileType.toLowerCase()) {
      case 'pdf': return 'fa-file-pdf';
      case 'doc':
      case 'docx': return 'fa-file-word';
      case 'xls':
      case 'xlsx': return 'fa-file-excel';
      case 'jpg':
      case 'jpeg':
      case 'png':
      case 'gif': return 'fa-file-image';
      case 'zip':
      case 'rar': return 'fa-file-archive';
      case 'txt': return 'fa-file-alt';
      default: return 'fa-file';
    }
  }
  openUploadModal(): void {
  this.selectedFiles = [];
  this.isOpeningUploadModal = true;
  
  // Tạo hiệu ứng hiển thị từ từ
  setTimeout(() => {
    this.showUploadModal = true;
  }, 50);
}

closeUploadModal(): void {
  // Tạo hiệu ứng mờ dần trước khi đóng
  this.isOpeningUploadModal = false;
  
  setTimeout(() => {
    this.showUploadModal = false;
  }, 300);
}

// Modal Folder
openNewFolderModal(): void {
  this.editMode = false;
  this.folderForm.reset({
    id: 0,
    employeeId: this.employeeId
  });
  
  this.isOpeningFolderModal = true;
  
  setTimeout(() => {
    this.showFolderModal = true;
  }, 50);
}

openEditFolderModal(): void {
  if (!this.selectedFolder) return;
  
  this.editMode = true;
  this.folderForm.patchValue({
    id: this.selectedFolder.id,
    folderName: this.selectedFolder.folderName,
    description: this.selectedFolder.description,
    employeeId: this.employeeId
  });
  
  this.isOpeningFolderModal = true;
  
  setTimeout(() => {
    this.showFolderModal = true;
  }, 50);
}

closeFolderModal(): void {
  this.isOpeningFolderModal = false;
  
  setTimeout(() => {
    this.showFolderModal = false;
  }, 300);
}

// Modal Document
openDocumentModal(document: EmployeeDocument): void {
  this.documentForm.patchValue({
    id: document.id,
    documentName: document.documentName,
    notes: document.notes,
    tags: document.tags,
    expiryDate: document.expiryDate ? this.formatDateToLocalISO(document.expiryDate) : null,
    folderId: document.folderId,
    employeeId: document.employeeId
  });

  this.isOpeningDocumentModal = true;

  setTimeout(() => {
    this.showDocumentModal = true;
  }, 50);
}

private formatDateToLocalISO(date: string | Date): string {
  const d = new Date(date);
  const year = d.getFullYear();
  const month = String(d.getMonth() + 1).padStart(2, '0');
  const day = String(d.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}


closeDocumentModal(): void {
  this.isOpeningDocumentModal = false;
  
  setTimeout(() => {
    this.showDocumentModal = false;
  }, 300);
}

// Cập nhật hàm saveFolder để sử dụng closeFolderModal
saveFolder(): void {
  if (this.folderForm.invalid) return;
  
  this.loading = true;
  const folderData = this.folderForm.value;
  
  const operation = this.editMode
    ? this.documentService.updateFolder(folderData)
    : this.documentService.createFolder(folderData);
  
  operation.subscribe({
    next: (res) => {
      if (res.success) {
        // Refresh folders
        this.loadFolders();
        
        // If we're in edit mode and folder was updated
        if (this.editMode && this.selectedFolder && folderData.id === this.selectedFolder.id) {
          this.selectedFolder = {...this.selectedFolder, ...folderData};
        }
        
        this.closeFolderModal();
      }
      this.loading = false;
    },
    error: (err) => {
      console.error('Error saving folder:', err);
      this.loading = false;
    }
  });
}

// Cập nhật hàm saveDocument để sử dụng closeDocumentModal
saveDocument(): void {
  if (this.documentForm.invalid) return;
  
  this.loading = true;
  const documentData = this.documentForm.value;
  
  this.documentService.updateDocument(documentData).subscribe({
    next: (res) => {
      if (res.success && this.selectedFolder) {
        this.loadDocuments(this.selectedFolder.id);
        this.closeDocumentModal();
      }
      this.loading = false;
    },
    error: (err) => {
      console.error('Error saving document:', err);
      this.loading = false;
    }
  });
}

// Cập nhật hàm uploadFiles để sử dụng closeUploadModal
uploadFiles(): void {
  if (!this.selectedFolder || this.selectedFiles.length === 0) return;
  
  this.loading = true;
  
  // Create document DTOs for each file
  const uploadPromises = this.selectedFiles.map(file => {
    const fileExt = file.name.split('.').pop()?.toLowerCase() || '';
    
    // Create a basic document DTO
    const docDTO: EmployeeDocumentDTO = {
      id: 0,
      documentName: file.name.split('.')[0], // Use filename without extension as default
      documentType: fileExt,
      folderId: this.selectedFolder!.id,
      employeeId: this.employeeId,
      tags: '',
      notes: '',
      expiryDate: null,
      actionType: 'A'
    };
    
    // Return observable from service call
    return this.documentService.createDocument(docDTO, file);
  });
  
  // Process all uploads using forkJoin
  
  forkJoin(uploadPromises).subscribe({
    next: () => {
      // Reload documents after all uploads are done
      if (this.selectedFolder) {
        this.loadDocuments(this.selectedFolder.id);
        // Also reload folders to update document count
        this.loadFolders();
      }
      this.closeUploadModal();
      this.loading = false;
    },
    error: (err) => {
      console.error('Error uploading files:', err);
      this.loading = false;
    }
  });
}
}