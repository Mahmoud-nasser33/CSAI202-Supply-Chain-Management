
document.addEventListener('mousemove', (e) => {
    const moveX = (e.clientX / window.innerWidth - 0.5) * 20;
    const moveY = (e.clientY / window.innerHeight - 0.5) * 20;

    document.body.style.setProperty('--mouse-x', `${moveX}px`);
    document.body.style.setProperty('--mouse-y', `${moveY}px`);
});

let ticking = false;
window.addEventListener('scroll', () => {
    if (!ticking) {
        window.requestAnimationFrame(() => {
            const scrolled = window.pageYOffset;
            const parallax = scrolled * 0.3;

            document.body.style.setProperty('--scroll-y', `${parallax}px`);
            ticking = false;
        });
        ticking = true;
    }
});
