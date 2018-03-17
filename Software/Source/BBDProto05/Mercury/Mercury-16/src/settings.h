/*
 * settings.h
 *
 * Created: 2017-03-07 16:16:03
 *  Author: Andras
 */ 


#ifndef SETTINGS_H_
#define SETTINGS_H_

#include <bbd_communication.h>

typedef void (*settings_changed_callback_t) (CellSettings_t* old_settings, CellSettings_t new_settings);

CellSettings_t settings;

void settings_load_defaults(void);
void settings_load(CellSettings_t new_settings);

settings_changed_callback_t settings_changed_callback;

#endif /* SETTINGS_H_ */