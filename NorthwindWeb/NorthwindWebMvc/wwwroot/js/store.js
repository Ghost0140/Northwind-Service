document.addEventListener('DOMContentLoaded', function () {
    // Validación de cantidad en formularios de agregar al carrito
    const quantityInputs = document.querySelectorAll('input[type="number"]');
    quantityInputs.forEach(input => {
        input.addEventListener('change', function () {
            const value = parseInt(this.value);
            const min = parseInt(this.min) || 1;
            const max = parseInt(this.max) || 999;
            
            if (value < min) this.value = min;
            if (value > max) this.value = max;
        });
    });

    // Confirmación para eliminar del carrito
    const deleteButtons = document.querySelectorAll('.delete-from-cart');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function (e) {
            if (!confirm('¿Deseas eliminar este producto del carrito?')) {
                e.preventDefault();
            }
        });
    });

    // Animación al agregar al carrito
    const addCartForms = document.querySelectorAll('form[action*="AddToCart"]');
    addCartForms.forEach(form => {
        form.addEventListener('submit', function () {
            const button = this.querySelector('button[type="submit"]');
            if (button) {
                const originalText = button.innerHTML;
                button.innerHTML = '<i class="bi bi-check-circle"></i> Agregado!';
                button.disabled = true;
                setTimeout(() => {
                    button.innerHTML = originalText;
                    button.disabled = false;
                }, 2000);
            }
        });
    });

    // Actualizar carrito en tiempo real (si es necesario)
    const checkoutForm = document.querySelector('form[action*="ProcessCheckout"]');
    if (checkoutForm) {
        checkoutForm.addEventListener('submit', function (e) {
            const fields = ['customerName', 'customerEmail', 'shippingAddress'];
            for (let field of fields) {
                const element = document.getElementById(field);
                if (element && !element.value.trim()) {
                    e.preventDefault();
                    alert(`Por favor completa el campo: ${element.previousElementSibling?.textContent || field}`);
                    return;
                }
            }
        });
    }
});
