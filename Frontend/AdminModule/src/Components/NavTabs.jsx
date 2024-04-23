// renders nav tab list
// list     - list of names to use as tabs
// update   - function to run when clicking on a tab
// active   - index of the active list item
const NavTabs = ({list, update, active}) => {

    return (
        <>
            <ul className="nav nav-tabs nav-tabs-recipe">
                {list.map((item, idx) => {
                    let style = idx == active ? " selected" : "";
                    return (
                        <li 
                            key={item}
                            className={"nav-item nav-item-recipe"+style}
                            onClick={() => update(item)}
                        >
                            <a className="nav-link nav-link-recipe" data-toggle="tab">{item}</a>
                        </li>
                    )
                })}
            </ul>
        </>
    )
}

export default NavTabs;