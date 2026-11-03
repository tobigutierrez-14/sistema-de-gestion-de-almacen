(() => {
  "use strict";
  const form = document.getElementById("ventaForm");
  if (!form) return;
  const $ = id => document.getElementById(id);
  const search = $("buscarProducto"), quantity = $("cantidadAgregar");
  const catalog = $("ventaCatalogo"), cartBox = $("ventaCarrito"), fields = $("ventaCampos");
  const client = $("ClienteId"), payment = $("MedioPagoId"), submit = $("confirmarVenta");
  const products = [...catalog.querySelectorAll(".venta-producto")];
  const catalogById = new Map(products.map(node => [Number(node.dataset.id), {
    id: Number(node.dataset.id), name: node.dataset.nombre, price: Number(node.dataset.precio),
    stock: Number(node.dataset.stock), node
  }]));
  const cart = new Map();
  const money = new Intl.NumberFormat("es-AR", { style: "currency", currency: "ARS" });
  const message = text => { $("ventaMensaje").textContent = text; };
  const newElement = (tag, className, text) => {
    const node = document.createElement(tag);
    if (className) node.className = className;
    if (text !== undefined) node.textContent = text;
    return node;
  };
  function render() {
    cartBox.replaceChildren(); fields.replaceChildren();
    let total = 0, units = 0;
    [...cart.entries()].forEach(([id, count], index) => {
      const product = catalogById.get(id);
      total += product.price * count; units += count;
      const row = newElement("div", "venta-carrito-fila");
      const info = newElement("div", "venta-carrito-info");
      info.append(newElement("strong", "", product.name), newElement("small", "", `${money.format(product.price)} / unidad`));
      const change = newElement("input", "venta-carrito-cantidad");
      change.type = "number"; change.min = "1"; change.max = String(product.stock); change.step = "1";
      change.value = String(count); change.setAttribute("aria-label", `Cantidad de ${product.name}`);
      change.addEventListener("change", () => {
        const next = Number(change.value);
        if (!Number.isSafeInteger(next) || next < 1 || next > product.stock) {
          message(`Ingresá entre 1 y ${product.stock} unidades para ${product.name}.`);
          change.value = String(cart.get(id)); return;
        }
        message(""); cart.set(id, next); render();
      });
      const subtotal = newElement("strong", "venta-carrito-subtotal", money.format(product.price * count));
      const remove = newElement("button", "venta-quitar", "Quitar"); remove.type = "button";
      remove.setAttribute("aria-label", `Quitar ${product.name}`);
      remove.addEventListener("click", () => { cart.delete(id); message(""); render(); });
      row.append(info, change, subtotal, remove); cartBox.append(row);
      for (const [key,value] of [["ProductoId",id],["Cantidad",count]]) {
        const hidden = document.createElement("input"); hidden.type = "hidden";
        hidden.name = `Lineas[${index}].${key}`; hidden.value = String(value);
        fields.append(hidden);
      }
    });
    $("carritoVacio").hidden = cart.size > 0;
    $("cantidadCarrito").textContent = `${cart.size} productos`;
    $("resumenItems").textContent = String(cart.size);
    $("resumenUnidades").textContent = String(units);
    $("resumenTotal").textContent = money.format(total);
    submit.disabled = cart.size === 0;
  }
  const selectedText = input => input.selectedIndex > 0 ? input.options[input.selectedIndex].textContent.trim() : "Sin seleccionar";
  function updateSummary() { $("resumenCliente").textContent = selectedText(client); $("resumenMedio").textContent = selectedText(payment); }
  client.addEventListener("change", updateSummary); payment.addEventListener("change", updateSummary);
  search.addEventListener("input", () => {
    const q = search.value.trim().toLocaleLowerCase("es-AR"); let visible = 0;
    products.forEach(p => {
      const matches = `${p.dataset.nombre} ${p.dataset.categoria}`.toLocaleLowerCase("es-AR").includes(q);
      p.hidden = !matches; if (matches) visible++;
    });
    $("catalogoVacio").hidden = visible > 0;
  });
  products.forEach(p => p.addEventListener("click", () => {
    const product = catalogById.get(Number(p.dataset.id));
    const count = Number(quantity.value);
    if (!Number.isSafeInteger(count) || count < 1) { message("Ingresá una cantidad válida mayor a cero."); return; }
    const next = (cart.get(product.id) || 0) + count;
    if (next > product.stock) { message(`Stock insuficiente de ${product.name}. Disponibles: ${product.stock}.`); return; }
    cart.set(product.id, next); message(`${product.name} agregado al carrito.`); render();
  }));
  // Recompone el carrito tras un error de validación del servidor.
  const previous = [...fields.querySelectorAll('input[name^="Lineas["]')];
  const lines = new Map();
  previous.forEach(input => {
    const match = input.name.match(/^Lineas\[(\d+)\]\.(ProductoId|Cantidad)$/);
    if (!match) return;
    const row = lines.get(match[1]) || {}; row[match[2]] = Number(input.value); lines.set(match[1], row);
  });
  lines.forEach(row => { if (catalogById.has(row.ProductoId) && row.Cantidad > 0) cart.set(row.ProductoId, row.Cantidad); });
  form.addEventListener("submit", event => {
    if (cart.size === 0) { event.preventDefault(); message("Agregá al menos un producto."); }
  });
  updateSummary(); render();
})();