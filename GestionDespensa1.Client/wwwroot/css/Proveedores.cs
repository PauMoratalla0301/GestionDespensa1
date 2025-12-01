/* Estilos personalizados para proveedores */
.opacity - 75 {
opacity: 0.75;
}

.table - hover tbody tr:hover {
    background-color: rgba(0, 123, 255, 0.05);
transform: translateY(-1px);
transition: all 0.2s ease;
}

.badge.bg - light {
border: 1px solid #dee2e6;
}

.card - header.bg - primary {
background: linear - gradient(135deg, #4e73df 0%, #224abe 100%);
}

.btn - group - sm.btn {
padding: 0.25rem 0.5rem;
    font - size: 0.875rem;
}

/* Estilos para badges de estado */
.badge.bg - success {
    background - color: #1cc88a !important;
}

.badge.bg - warning {
    background - color: #f6c23e !important;
    color: #000;
}

/* Animaciones de carga */
.fa - spinner {
animation: spin 1s linear infinite;
}

@keyframes spin
{
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}

/* Responsive */
@media(max - width: 768px) {
    .table - responsive {
        font - size: 0.875rem;
    }
    
    .btn - group {
        flex - direction: column;
    gap: 2px;
    }
    
    .btn - group.btn {
    margin: 1px;
    width: 100 %;
    }
}