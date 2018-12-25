const customerListIndexTemplate = {
    normalMode: `<body>
                    <div class="lnk-changetab"></div>
                    <div class="form-inline"></div>
                    <input id="directDownline" value="2" />
                    <input id="Open" value="Open" />
                    <input id="Closed" value="Closed" />
                    <input id="Suspended" value="Suspended" />
                    <div class="update-credit" cust-id='1'>200</div>
                    <div class="update-status" cust-id='1'>3</div>
                </body>`,
    masterTab: `<body>
                    <div class="lnk-changetab"></div>
                    <div class="form-inline"></div>
                    <input id="directDownline" value="3" />
                </body>`,
    agentTab: `<body>
                    <div class="lnk-changetab"></div>
                    <div class="form-inline"></div>
                    <input id="directDownline" value="2" />
                    <div class="tab-content" level="2"></div>
                </body>`,
    memberTab: `<body>
                    <div class="lnk-changetab"></div>
                    <div class="form-inline"></div>
                    <input id="directDownline" value="1" />
                    <div class="tab-content" level="1"></div>
                </body>`
};