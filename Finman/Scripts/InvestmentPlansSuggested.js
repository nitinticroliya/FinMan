
function func1(val, i) {
    var inv = "Inv";
    var number = document.getElementById("RetirementDetails").rows.length / 4;
    if (document.getElementById(inv+"DetailsA_" + i).style.display == "table-row") {
        document.getElementById(inv + "DetailsA_" + i).style.display = "none";
    }
    else {
        document.getElementById(inv + "DetailsA_" + i).style.display = "table-row";
    }
    if (document.getElementById(inv + "DetailsB_" + i).style.display == "table-row") {
        document.getElementById(inv + "DetailsB_" + i).style.display = "none";
    }
    else {
        document.getElementById(inv + "DetailsB_" + i).style.display = "table-row";
    }
    if (document.getElementById(inv + "DetailsC_" + i).style.display == "table-row") {
        document.getElementById(inv + "DetailsC_" + i).style.display = "none";
    }
    else {
        document.getElementById(inv + "DetailsC_" + i).style.display = "table-row";
    }
    console.log(val);
}
function func2(val, i) {
    var inv = "Ret";
    var number = document.getElementById("RetirementDetails").rows.length / 4;
    if (document.getElementById(inv + "DetailsA_" + i).style.display == "table-row") {
        document.getElementById(inv + "DetailsA_" + i).style.display = "none";
    }
    else {
        document.getElementById(inv + "DetailsA_" + i).style.display = "table-row";
    }
    if (document.getElementById(inv + "DetailsB_" + i).style.display == "table-row") {
        document.getElementById(inv + "DetailsB_" + i).style.display = "none";
    }
    else {
        document.getElementById(inv + "DetailsB_" + i).style.display = "table-row";
    }
    if (document.getElementById(inv + "DetailsC_" + i).style.display == "table-row") {
        document.getElementById(inv + "DetailsC_" + i).style.display = "none";
    }
    else {
        document.getElementById(inv + "DetailsC_" + i).style.display = "table-row";
    }
    console.log(val);
}