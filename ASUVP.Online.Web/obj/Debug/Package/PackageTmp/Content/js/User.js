
function OnEditUserClick(s, e) {
    window.open(ROOT_URL + "User/Edit/"+e);
}

function OnEditUserCompaniesClick(companyId, userId) {
    window.open(ROOT_URL + "User/UserCompanyRolesEdit?companyId=" + companyId + "&userId=" + userId);
}