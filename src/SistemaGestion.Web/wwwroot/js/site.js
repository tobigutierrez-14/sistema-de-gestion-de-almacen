(() => {
  const button = document.querySelector('#menuToggle');
  const backdrop = document.querySelector('#sidebarBackdrop');
  const sidebar = document.querySelector('#sidebar');
  if (!button || !backdrop || !sidebar) return;
  function setOpen(open) {
    document.body.classList.toggle('sidebar-open', open);
    button.setAttribute('aria-expanded', String(open));
    button.setAttribute('aria-label', open ? 'Cerrar menú' : 'Abrir menú');
  }
  button.addEventListener('click', () => setOpen(!document.body.classList.contains('sidebar-open')));
  backdrop.addEventListener('click', () => setOpen(false));
  document.addEventListener('keydown', (event) => { if (event.key === 'Escape') setOpen(false); });
  sidebar.querySelectorAll('a').forEach(link => link.addEventListener('click', () => setOpen(false)));
  window.matchMedia('(min-width: 961px)').addEventListener('change', e => { if (e.matches) setOpen(false); });
})();
