/// <summary>
/// Derrick Nagy
/// Created: 2022/04/07
/// 
/// Description:
/// sets a label attribute on all td tags
/// ripped off from https://medium.com/allenhwkim/mobile-friendly-table-b0cb066dbc0e
/// </summary>
window.setMobileTable = function (selector) {
    // if (window.innerWidth > 600) return false;
    const tableEl = document.querySelector(selector);
    const thEls = tableEl.querySelectorAll('thead th');
    const tdLabels = Array.from(thEls).map(el => el.innerText);
    tableEl.querySelectorAll('tbody tr').forEach(tr => {
        Array.from(tr.children).forEach(
            (td, ndx) => td.setAttribute('label', tdLabels[ndx])
        );
    });
}